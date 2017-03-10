using StaRTS.Assets;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Controllers.Objectives;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Controllers.World.Transitions;
using StaRTS.Main.Models.Commands.Planets;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Animations;
using StaRTS.Main.Views.Cameras;
using StaRTS.Main.Views.UserInput;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Planets
{
	public class PlanetRelocationController
	{
		private AssetManager assetManager;

		private AssetHandle hyperspaceHandle;

		private AssetHandle destinationHandle;

		private AssetHandle currentPlanetHandle;

		private AssetHandle assetHandlePlanetGlow;

		private AssetHandle assetHandleRelocationAudio;

		private GameObject hyperspaceGameObject;

		private GameObject planetGlowGameObject;

		private GameObject destinationPlanetGameObject;

		private GameObject currentPlanetGameObject;

		private bool relocating;

		private bool relocationInProgress;

		private const string START_HYPERSPACE = "StartHyperspace";

		private const string CURRENT_PLANET_ANCHOR = "HomePlanet";

		private const string DESTINATION_PLANET_ANCHOR = "DestinationPlanet";

		private const string PREFAB_LOAD_ERROR = "Invalid GameObject: ";

		private TransitionVisuals transitionVisuals;

		private PlanetVO destinationVO;

		private bool currentPlanetLoadReturned;

		private bool destinationPlanetLoadReturned;

		private bool planetGlowAssetLoadReturned;

		private bool hyperspaceLoadReturned;

		private bool relocationAudioReturned;

		private CameraManager cameraManager;

		private Material planetMaterial;

		private bool LOAD_CURRENT_PLANET;

		public PlanetRelocationController()
		{
			Service.Set<PlanetRelocationController>(this);
			this.assetManager = Service.Get<AssetManager>();
			this.cameraManager = Service.Get<CameraManager>();
			this.relocating = false;
			this.relocationInProgress = false;
		}

		public void SendRelocationRequest(PlanetVO planetVO, bool payHardCurrency)
		{
			Service.Get<UserInputManager>().Enable(false);
			this.relocating = false;
			this.relocationInProgress = true;
			Service.Get<EventManager>().SendEvent(EventId.PlanetConfirmRelocate, planetVO.Uid);
			if (this.destinationVO != null)
			{
				Service.Get<UserInputManager>().Enable(true);
				Service.Get<StaRTSLogger>().Error("Relocation request for " + planetVO.Uid + " before previous finished.");
				return;
			}
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			if (planetVO != null && currentPlayer.UnlockedPlanets.Contains(planetVO.Uid))
			{
				this.destinationVO = planetVO;
				RelocatePlanetRequest request = new RelocatePlanetRequest(planetVO.Uid, payHardCurrency);
				PlanetRelocationCommand planetRelocationCommand = new PlanetRelocationCommand(request);
				planetRelocationCommand.AddSuccessCallback(new AbstractCommand<RelocatePlanetRequest, DefaultResponse>.OnSuccessCallback(this.OnRelocateSuccess));
				planetRelocationCommand.AddFailureCallback(new AbstractCommand<RelocatePlanetRequest, DefaultResponse>.OnFailureCallback(this.OnRelocateFail));
				Service.Get<ServerAPI>().Sync(planetRelocationCommand);
				return;
			}
			Service.Get<UserInputManager>().Enable(true);
			Service.Get<StaRTSLogger>().Error("Invalid relocation request.");
		}

		private void OnRelocateSuccess(DefaultResponse response, object cookie)
		{
			this.currentPlanetLoadReturned = false;
			this.destinationPlanetLoadReturned = false;
			this.hyperspaceLoadReturned = false;
			this.planetGlowAssetLoadReturned = false;
			this.relocationAudioReturned = false;
			this.assetManager.Load(ref this.hyperspaceHandle, "planets_lightspeed_transition", new AssetSuccessDelegate(this.HyperSpaceLoadSuccess), new AssetFailureDelegate(this.HyperspaceLoadFail), null);
			if (this.LOAD_CURRENT_PLANET)
			{
				this.assetManager.Load(ref this.currentPlanetHandle, Service.Get<CurrentPlayer>().Planet.GalaxyAssetName, new AssetSuccessDelegate(this.CurrentPlanetLoadSuccess), new AssetFailureDelegate(this.CurrentPlanetLoadFail), null);
			}
			this.assetManager.Load(ref this.destinationHandle, this.destinationVO.GalaxyAssetName, new AssetSuccessDelegate(this.DestinationPlanetLoadSuccess), new AssetFailureDelegate(this.DestinationPlanetLoadFail), null);
			this.assetManager.Load(ref this.assetHandlePlanetGlow, "fx_planetGlow", new AssetSuccessDelegate(this.PlanetGlowLoaded), new AssetFailureDelegate(this.LoadingPlanetGlowFail), null);
			this.assetManager.Load(ref this.assetHandleRelocationAudio, "sfx_trans_hyperspace", new AssetSuccessDelegate(this.RelocationAudioLoaded), new AssetFailureDelegate(this.RelocationAudioFail), null);
		}

		private void HyperSpaceLoadSuccess(object asset, object cookie)
		{
			this.hyperspaceLoadReturned = true;
			this.hyperspaceGameObject = (asset as GameObject);
			if (this.hyperspaceGameObject == null)
			{
				Service.Get<StaRTSLogger>().Warn("Invalid GameObject: planets_lightspeed_transition");
			}
			else
			{
				this.cameraManager.HyperspaceCamera = this.hyperspaceGameObject.GetComponentInChildren<Camera>();
				this.cameraManager.HyperspaceCamera.enabled = false;
			}
			this.AssembleHyperSpace();
		}

		private void HyperspaceLoadFail(object cookie)
		{
			this.hyperspaceLoadReturned = true;
			this.AssembleHyperSpace();
		}

		private void CurrentPlanetLoadSuccess(object asset, object cookie)
		{
			this.currentPlanetLoadReturned = true;
			this.currentPlanetGameObject = (asset as GameObject);
			if (this.currentPlanetGameObject == null)
			{
				Service.Get<StaRTSLogger>().Warn("Invalid GameObject: " + Service.Get<CurrentPlayer>().Planet.GalaxyAssetName);
			}
			this.AssembleHyperSpace();
		}

		private void CurrentPlanetLoadFail(object cookie)
		{
			this.currentPlanetLoadReturned = true;
			this.AssembleHyperSpace();
		}

		private void PlanetGlowLoaded(object asset, object cookie)
		{
			this.planetGlowGameObject = (asset as GameObject);
			this.planetGlowGameObject = UnityEngine.Object.Instantiate<GameObject>(this.planetGlowGameObject);
			if (this.planetGlowGameObject == null)
			{
				Service.Get<StaRTSLogger>().Warn("Invalid GameObject: fx_planetGlow");
			}
			this.planetGlowAssetLoadReturned = true;
			this.AssembleHyperSpace();
		}

		private void LoadingPlanetGlowFail(object cookie)
		{
			this.planetGlowAssetLoadReturned = true;
			this.AssembleHyperSpace();
		}

		private void RelocationAudioLoaded(object asset, object cookie)
		{
			this.relocationAudioReturned = true;
			this.AssembleHyperSpace();
		}

		private void RelocationAudioFail(object cookie)
		{
			this.relocationAudioReturned = true;
			this.AssembleHyperSpace();
		}

		private void DestinationPlanetLoadSuccess(object asset, object cookie)
		{
			this.destinationPlanetLoadReturned = true;
			this.destinationPlanetGameObject = (asset as GameObject);
			if (this.destinationPlanetGameObject == null)
			{
				Service.Get<StaRTSLogger>().Warn("Invalid GameObject: " + this.destinationVO.GalaxyAssetName);
			}
			else
			{
				this.planetMaterial = PlanetUtils.StopPlanetSpinning(this.destinationPlanetGameObject);
			}
			this.AssembleHyperSpace();
		}

		private void DestinationPlanetLoadFail(object cookie)
		{
			this.destinationPlanetLoadReturned = true;
			this.AssembleHyperSpace();
		}

		private void OnRelocateFail(uint status, object cookie)
		{
			Service.Get<StaRTSLogger>().Warn(string.Concat(new object[]
			{
				"Planet Relocation Request failed: ",
				status,
				" cookie:",
				cookie
			}));
			Service.Get<UserInputManager>().Enable(true);
			this.CleanUp();
		}

		private void AssembleHyperSpace()
		{
			if (!this.hyperspaceLoadReturned || !this.destinationPlanetLoadReturned || !this.planetGlowAssetLoadReturned || !this.relocationAudioReturned || (!this.currentPlanetLoadReturned && this.LOAD_CURRENT_PLANET))
			{
				return;
			}
			if (this.hyperspaceGameObject != null)
			{
				GameObject gameObject = UnityUtils.FindGameObject(this.hyperspaceGameObject, "DestinationPlanet");
				this.hyperspaceGameObject.transform.position = new Vector3(5000f, 5000f, 0f);
				this.cameraManager.HyperspaceCamera.depth = 1f;
				this.cameraManager.HyperspaceCamera.enabled = true;
				if (this.destinationPlanetGameObject != null)
				{
					Transform transform = this.destinationPlanetGameObject.transform;
					transform.parent = gameObject.transform;
					transform.localPosition = Vector3.zero;
					transform.localRotation = Quaternion.identity;
					transform.localScale = Vector3.one;
					Service.Get<GalaxyPlanetController>().SetPlanetLighting(this.destinationPlanetGameObject, this.planetMaterial, this.cameraManager.HyperspaceCamera);
				}
				if (this.planetGlowGameObject != null)
				{
					this.planetGlowGameObject.SetActive(true);
					UnityUtils.SetLayerRecursively(this.planetGlowGameObject, 9);
					Transform transform2 = this.planetGlowGameObject.transform;
					transform2.parent = gameObject.transform;
					transform2.localPosition = Vector3.zero;
					transform2.localRotation = Quaternion.identity;
					transform2.localScale = Vector3.one;
					transform2.LookAt(this.cameraManager.HyperspaceCamera.GetComponent<Camera>().transform.position);
				}
				if (this.LOAD_CURRENT_PLANET && this.currentPlanetGameObject != null)
				{
					GameObject gameObject2 = UnityUtils.FindGameObject(this.hyperspaceGameObject, "HomePlanet");
					Transform transform3 = this.currentPlanetGameObject.transform;
					transform3.parent = gameObject2.transform;
					transform3.localPosition = Vector3.zero;
					transform3.localRotation = Quaternion.identity;
					transform3.localScale = Vector3.one;
				}
			}
			this.cameraManager.WipeCamera.StartLinearWipe(WipeTransition.FromGalaxyToHyperspace, 1.57079637f, new WipeCompleteDelegate(this.WipeFromGalaxyToHyperspaceComplete), null);
		}

		private void WipeFromGalaxyToHyperspaceComplete(object cookie)
		{
			this.cameraManager.UXSceneCamera.Camera.clearFlags = CameraClearFlags.Depth;
			Animator componentInChildren = this.hyperspaceGameObject.GetComponentInChildren<Animator>();
			Service.Get<UserInputManager>().Enable(false);
			if (componentInChildren == null)
			{
				Service.Get<StaRTSLogger>().Warn("Hyperspace animator missing.");
				return;
			}
			componentInChildren.SetTrigger("StartHyperspace");
			Service.Get<EventManager>().SendEvent(EventId.PlanetRelocateStarted, null);
		}

		public void HyperspaceComplete()
		{
			this.transitionVisuals = new TransitionVisuals(this.destinationVO, new TransitionVisualsLoadedDelegate(this.UILoaded), null, true);
		}

		private void UILoaded(object cookie)
		{
			this.InternalRelocate();
		}

		private void InternalRelocate()
		{
			if (this.relocating)
			{
				return;
			}
			this.relocating = true;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			currentPlayer.Map.Planet = this.destinationVO;
			Service.Get<GalaxyViewController>().GoToHome(false, null, null);
			currentPlayer.Map.ReinitializePlanet(this.destinationVO.Uid);
			Service.Get<ObjectiveManager>().Relocate();
			Service.Get<SquadController>().SyncCurrentPlayerPlanet();
			Service.Get<WorldTransitioner>().StartTransition(new WorldToWorldTransition(null, Service.Get<HomeMapDataLoader>(), new TransitionCompleteDelegate(this.RelocationComplete), true, false));
		}

		private void RelocationComplete()
		{
			this.cameraManager.WipeCamera.StartLinearWipe(WipeTransition.FromHyperspaceToBase, 1.57079637f, new WipeCompleteDelegate(this.WipeFromHyperSpaceToPlanetComplete), null);
		}

		private void WipeFromHyperSpaceToPlanetComplete(object cookie)
		{
			this.cameraManager.UXSceneCamera.Camera.clearFlags = CameraClearFlags.Color;
			Service.Get<UXController>().HUD.Visible = true;
			this.cameraManager.UXCamera.Camera.enabled = true;
			Service.Get<EventManager>().SendEvent(EventId.PlanetRelocate, this.destinationVO.Uid);
			this.CleanUp();
		}

		private void CleanUp()
		{
			this.destinationVO = null;
			if (this.hyperspaceHandle != AssetHandle.Invalid)
			{
				this.assetManager.Unload(this.hyperspaceHandle);
				this.hyperspaceHandle = AssetHandle.Invalid;
			}
			if (this.destinationHandle != AssetHandle.Invalid)
			{
				this.assetManager.Unload(this.destinationHandle);
				this.destinationHandle = AssetHandle.Invalid;
			}
			if (this.currentPlanetHandle != AssetHandle.Invalid)
			{
				this.assetManager.Unload(this.currentPlanetHandle);
				this.currentPlanetHandle = AssetHandle.Invalid;
			}
			if (this.currentPlanetGameObject != null)
			{
				UnityEngine.Object.Destroy(this.currentPlanetGameObject);
				this.currentPlanetGameObject = null;
			}
			if (this.destinationPlanetGameObject != null)
			{
				UnityEngine.Object.Destroy(this.destinationPlanetGameObject);
				this.destinationPlanetGameObject = null;
			}
			if (this.planetGlowGameObject != null)
			{
				UnityEngine.Object.Destroy(this.planetGlowGameObject);
				this.planetGlowGameObject = null;
			}
			if (this.assetHandlePlanetGlow != AssetHandle.Invalid)
			{
				this.assetManager.Unload(this.assetHandlePlanetGlow);
				this.assetHandlePlanetGlow = AssetHandle.Invalid;
			}
			if (this.assetHandleRelocationAudio != AssetHandle.Invalid)
			{
				this.assetManager.Unload(this.assetHandleRelocationAudio);
				this.assetHandleRelocationAudio = AssetHandle.Invalid;
			}
			if (this.hyperspaceGameObject != null)
			{
				UnityEngine.Object.Destroy(this.hyperspaceGameObject);
				this.hyperspaceGameObject = null;
			}
			if (this.transitionVisuals != null)
			{
				this.transitionVisuals.Cleanup();
				this.transitionVisuals = null;
			}
			UnityUtils.DestroyMaterial(this.planetMaterial);
			this.relocationInProgress = false;
		}

		public bool IsRelocationInProgress()
		{
			return this.relocationInProgress;
		}

		protected internal PlanetRelocationController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).AssembleHyperSpace();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).CurrentPlanetLoadFail(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).CurrentPlanetLoadSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).DestinationPlanetLoadFail(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).DestinationPlanetLoadSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).HyperspaceComplete();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).HyperspaceLoadFail(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).HyperSpaceLoadSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).InternalRelocate();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).IsRelocationInProgress());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).LoadingPlanetGlowFail(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).OnRelocateSuccess((DefaultResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).PlanetGlowLoaded(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).RelocationAudioFail(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).RelocationAudioLoaded(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).RelocationComplete();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).SendRelocationRequest((PlanetVO)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).UILoaded(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).WipeFromGalaxyToHyperspaceComplete(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((PlanetRelocationController)GCHandledObjects.GCHandleToObject(instance)).WipeFromHyperSpaceToPlanetComplete(GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
