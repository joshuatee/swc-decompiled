using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.Planets;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Animations;
using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Main.Views.World;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers.World.Transitions
{
	public class AbstractTransition : IEventObserver
	{
		protected const string NOT_DOING_SOFT_WIPE = "You must already be in a soft wipe to call for a wipe to continue.";

		protected IMapDataLoader mapDataLoader;

		protected bool transitioning;

		protected bool skipTransitions;

		protected string alertMessage;

		protected bool softWipe;

		protected bool softWipeComplete;

		protected bool softWipeWorldLoad;

		protected bool softWipeAutoFinish;

		protected bool zoomOut;

		protected TransitionCompleteDelegate onTransitionComplete;

		protected TransitionInStartDelegate onTransitionInStart;

		protected TransitionVisuals transitionVisuals;

		protected float wipeDirection;

		protected IState transitionToState;

		protected bool worldLoaded;

		protected bool preloadsLoaded;

		protected WipeTransition startWipeTransition;

		protected WipeTransition endWipeTransition;

		protected AbstractTransition(IState transitionToState, IMapDataLoader mapDataLoader, TransitionCompleteDelegate onTransitionComplete, bool skipTransitions, bool zoomOut, WipeTransition startWipeTransition, WipeTransition endWipeTransition)
		{
			this.transitioning = false;
			this.softWipe = false;
			this.softWipeComplete = false;
			this.softWipeWorldLoad = false;
			this.softWipeAutoFinish = false;
			this.wipeDirection = 0f;
			this.transitionToState = transitionToState;
			this.mapDataLoader = mapDataLoader;
			this.onTransitionComplete = onTransitionComplete;
			this.skipTransitions = skipTransitions;
			this.zoomOut = zoomOut;
			this.startWipeTransition = startWipeTransition;
			this.endWipeTransition = endWipeTransition;
			if (!this.softWipe && transitionToState != null)
			{
				this.ChooseRandomWipeDirection();
			}
		}

		public virtual void StartTransition()
		{
			this.transitioning = true;
			this.worldLoaded = false;
			this.preloadsLoaded = false;
			Service.Get<EventManager>().SendEvent(EventId.HideAllHolograms, null);
			if ((!this.softWipe && !this.skipTransitions) || this.softWipe)
			{
				this.StartTransitionOut(this.mapDataLoader.GetPlanetData(), false, this.zoomOut);
				return;
			}
			this.OnTransitionOutComplete(null);
		}

		protected void OnTransitionVisualsLoaded(object cookie)
		{
			bool flag = (bool)cookie;
			WipeCompleteDelegate completeCallback;
			if (flag)
			{
				completeCallback = new WipeCompleteDelegate(this.OnSoftWipeComplete);
			}
			else
			{
				completeCallback = new WipeCompleteDelegate(this.OnTransitionOutComplete);
			}
			Service.Get<CameraManager>().WipeCamera.StartLinearWipe(this.startWipeTransition, this.wipeDirection, completeCallback, null);
		}

		protected void OnSoftWipeComplete(object cookie)
		{
			this.softWipeComplete = true;
			if (this.softWipeWorldLoad)
			{
				this.OnTransitionOutComplete(null);
				this.softWipeWorldLoad = false;
				return;
			}
			if (this.softWipeAutoFinish)
			{
				this.StartTransitionIn();
				this.softWipeAutoFinish = false;
			}
		}

		protected void StartTransitionOut(PlanetVO planetVO, bool forSoftWipe, bool zoomOut)
		{
			if (this.softWipe)
			{
				if (this.softWipeComplete)
				{
					this.OnTransitionOutComplete(null);
				}
				else
				{
					this.softWipeWorldLoad = true;
				}
			}
			else
			{
				this.CleanupTransitionVisuals();
				if (planetVO == null)
				{
					planetVO = Service.Get<IDataController>().Get<PlanetVO>("planet1");
				}
				this.transitionVisuals = new TransitionVisuals(planetVO, new TransitionVisualsLoadedDelegate(this.OnTransitionVisualsLoaded), forSoftWipe, false);
				if (zoomOut)
				{
					Service.Get<WorldInitializer>().View.ZoomOut();
				}
			}
			this.softWipe = forSoftWipe;
		}

		protected virtual void StartTransitionIn()
		{
			Service.Get<ScreenController>().CloseAll();
			this.StartTransitionInContinueSetup();
		}

		protected virtual void StartTransitionInContinueSetup()
		{
			if (this.onTransitionInStart != null)
			{
				this.onTransitionInStart();
				this.onTransitionInStart = null;
			}
			Service.Get<UXController>().MiscElementsManager.HideHighlight();
			Service.Get<GalaxyViewController>().ResetCameraForBase();
			Service.Get<WorldInitializer>().View.ResetCameraImmediate();
			CameraManager cameraManager = Service.Get<CameraManager>();
			cameraManager.MainCamera.ForceCameraMoveFinish();
			float wipeAngle = this.wipeDirection + 3.14159274f;
			cameraManager.WipeCamera.StartLinearWipe(this.endWipeTransition, wipeAngle, new WipeCompleteDelegate(this.OnTransitionInComplete), null);
			if (!Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
			{
				Service.Get<UserInputInhibitor>().AllowAll();
				Service.Get<WorldInitializer>().View.ResetCameraImmediate();
			}
			Service.Get<WorldInitializer>().View.ZoomOutImmediate();
			Service.Get<WorldInitializer>().View.ZoomTo(0.7f);
			this.softWipe = false;
		}

		public void StartWipe(PlanetVO planetVO)
		{
			if (!this.softWipe)
			{
				this.skipTransitions = false;
				this.ChooseRandomWipeDirection();
				this.softWipeComplete = false;
				this.softWipeWorldLoad = false;
				this.StartTransitionOut(planetVO, true, false);
			}
		}

		public void ContinueWipe(IState transitionToState, IMapDataLoader mapDataLoader, TransitionCompleteDelegate onTransitionComplete)
		{
			if (!this.softWipe)
			{
				Service.Get<StaRTSLogger>().Error("You must already be in a soft wipe to call for a wipe to continue.");
				return;
			}
			this.transitionToState = transitionToState;
			this.mapDataLoader = mapDataLoader;
			this.onTransitionComplete = onTransitionComplete;
			this.StartTransition();
		}

		public void FinishWipe()
		{
			if (this.softWipe)
			{
				if (this.softWipeComplete)
				{
					this.StartTransitionIn();
					return;
				}
				this.softWipeAutoFinish = true;
			}
		}

		protected void ChooseRandomWipeDirection()
		{
			this.wipeDirection = (float)Service.Get<Rand>().ViewRangeInt(0, 8) * 0.7853982f;
		}

		protected void OnTransitionOutComplete(object cookie)
		{
			Service.Get<EventManager>().SendEvent(EventId.WorldOutTransitionComplete, null);
			Service.Get<ScreenController>().CloseAll();
			if (this.transitionToState != null)
			{
				Service.Get<GameStateMachine>().SetState(this.transitionToState);
				this.transitionToState = null;
			}
			this.PreloadAssetsBeforeMapLoad();
			Service.Get<UserInputManager>().Enable(false);
		}

		protected void OnMapLoaded(Map map)
		{
			this.PreloadAssetsAfterMapLoad(map);
			Service.Get<EventManager>().RegisterObserver(this, EventId.WorldLoadComplete);
			Service.Get<WorldInitializer>().PrepareWorld(map);
		}

		protected void OnMapLoadFailure()
		{
			Service.Get<CameraManager>().UXCamera.Camera.enabled = true;
			Service.Get<UXController>().HUD.Visible = false;
			if (Service.Get<GameStateMachine>().CurrentState is HomeState)
			{
				this.FinishWipe();
				return;
			}
			HomeState.GoToHomeState(null, false);
		}

		protected void PreloadAssetsBeforeMapLoad()
		{
			Service.Get<ProjectileViewManager>().UnloadProjectileAssetsAndPools();
			if (Service.IsSet<SpecialAttackController>())
			{
				Service.Get<SpecialAttackController>().UnloadPreloads();
			}
			this.Preload();
		}

		protected void PreloadAssetsAfterMapLoad(Map map)
		{
			List<IAssetVO> projectilePreloads = this.mapDataLoader.GetProjectilePreloads(map);
			Service.Get<ProjectileViewManager>().LoadProjectileAssetsAndCreatePools(projectilePreloads);
			if (projectilePreloads != null && projectilePreloads.Count > 0 && Service.IsSet<SpecialAttackController>())
			{
				Service.Get<SpecialAttackController>().PreloadSpecialAttackMiscAssets();
			}
		}

		protected void Preload()
		{
			Service.Get<WorldPreloader>().Unload();
			Service.Get<Engine>().ForceGarbageCollection(new Action(this.OnGarbageCollected));
		}

		protected void OnGarbageCollected()
		{
			Service.Get<WorldPreloader>().Load(this.mapDataLoader.GetPreloads(), new WorldPreloader.PreloadSuccessDelegate(this.OnPreloadComplete));
		}

		protected void OnPreloadComplete()
		{
			this.preloadsLoaded = true;
			this.mapDataLoader.LoadMapData(new MapLoadedDelegate(this.OnMapLoaded), new MapLoadFailDelegate(this.OnMapLoadFailure));
		}

		protected void TryCompleteTransition()
		{
			if (!this.IsEverythingLoaded())
			{
				return;
			}
			if (!this.skipTransitions)
			{
				this.StartTransitionIn();
				return;
			}
			this.OnTransitionInComplete(null);
		}

		public bool IsEverythingLoaded()
		{
			return this.worldLoaded && this.preloadsLoaded;
		}

		protected virtual void OnTransitionInComplete(object cookie)
		{
			this.transitioning = false;
			if (this.onTransitionComplete != null)
			{
				this.onTransitionComplete();
				this.onTransitionComplete = null;
			}
			if (!this.skipTransitions)
			{
				Service.Get<EventManager>().SendEvent(EventId.WorldInTransitionComplete, null);
			}
			this.CleanupTransitionVisuals();
			if (this.alertMessage != null)
			{
				AlertScreen.ShowModal(false, null, this.alertMessage, null, null);
				this.alertMessage = null;
			}
			Service.Get<Engine>().ForceGarbageCollection(null);
		}

		protected void CleanupTransitionVisuals()
		{
			if (this.transitionVisuals != null)
			{
				this.transitionVisuals.Cleanup();
				this.transitionVisuals = null;
			}
		}

		public virtual EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.WorldLoadComplete)
			{
				this.worldLoaded = true;
				this.TryCompleteTransition();
				Service.Get<EventManager>().UnregisterObserver(this, EventId.WorldLoadComplete);
			}
			return EatResponse.NotEaten;
		}

		public bool IsCurrentWorldHome()
		{
			return this.mapDataLoader.GetWorldType() == WorldType.Home;
		}

		public bool IsCurrentWorldUserWarBase()
		{
			return this.mapDataLoader.GetWorldType() == WorldType.WarBase;
		}

		public string GetCurrentWorldName()
		{
			return this.mapDataLoader.GetWorldName();
		}

		public string GetCurrentWorldFactionAssetName()
		{
			return this.mapDataLoader.GetFactionAssetName();
		}

		public bool IsTransitioning()
		{
			return this.transitioning;
		}

		public bool IsSoftWipeing()
		{
			return this.softWipe;
		}

		public void SetOnTransitionComplete(TransitionCompleteDelegate onTransitionComplete)
		{
			this.onTransitionComplete = onTransitionComplete;
		}

		public void SetOnTransitionInStart(TransitionInStartDelegate onTransitionInStart)
		{
			this.onTransitionInStart = onTransitionInStart;
		}

		public void SetSkipTransitions(bool skip)
		{
			this.skipTransitions = skip;
		}

		public void SetAlertMessage(string alertMessage)
		{
			this.alertMessage = alertMessage;
		}

		public IMapDataLoader GetMapDataLoader()
		{
			return this.mapDataLoader;
		}

		protected internal AbstractTransition(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).ChooseRandomWipeDirection();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).CleanupTransitionVisuals();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).ContinueWipe((IState)GCHandledObjects.GCHandleToObject(*args), (IMapDataLoader)GCHandledObjects.GCHandleToObject(args[1]), (TransitionCompleteDelegate)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).FinishWipe();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).GetCurrentWorldFactionAssetName());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).GetCurrentWorldName());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).GetMapDataLoader());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).IsCurrentWorldHome());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).IsCurrentWorldUserWarBase());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).IsEverythingLoaded());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).IsSoftWipeing());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).IsTransitioning());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).OnGarbageCollected();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).OnMapLoaded((Map)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).OnMapLoadFailure();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).OnPreloadComplete();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).OnSoftWipeComplete(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).OnTransitionInComplete(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).OnTransitionOutComplete(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).OnTransitionVisualsLoaded(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).Preload();
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).PreloadAssetsAfterMapLoad((Map)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).PreloadAssetsBeforeMapLoad();
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).SetAlertMessage(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).SetOnTransitionComplete((TransitionCompleteDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).SetOnTransitionInStart((TransitionInStartDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).SetSkipTransitions(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).StartTransition();
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).StartTransitionIn();
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).StartTransitionInContinueSetup();
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).StartTransitionOut((PlanetVO)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).StartWipe((PlanetVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((AbstractTransition)GCHandledObjects.GCHandleToObject(instance)).TryCompleteTransition();
			return -1L;
		}
	}
}
