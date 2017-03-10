using StaRTS.Main.Controllers.World.Transitions;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.State;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers.World
{
	public class WorldTransitioner
	{
		private const string START_TRANSITION_ERROR = "Transition in progress, unable to set another transition.";

		private const string TRANSITION_IN_PROGRESS_WIPE_ERROR = "Transition in progress, unable to start another wipe.";

		private const string TRANSITION_NOT_SET_ERROR = "WorldTransitioner.currentTransition is not set.";

		public const string DEFAULT_PLANET = "planet1";

		private AbstractTransition currentTransition;

		public WorldTransitioner()
		{
			Service.Set<WorldTransitioner>(this);
		}

		public void StartTransition(AbstractTransition transition)
		{
			if (this.currentTransition != null && this.currentTransition.IsTransitioning())
			{
				Service.Get<StaRTSLogger>().Error("Transition in progress, unable to set another transition.");
				return;
			}
			this.currentTransition = transition;
			this.currentTransition.StartTransition();
		}

		public void StartWipe(AbstractTransition transition)
		{
			this.StartWipe(transition, Service.Get<IDataController>().Get<PlanetVO>("planet1"));
		}

		public void StartWipe(AbstractTransition transition, PlanetVO planetVO)
		{
			if (this.currentTransition != null && this.currentTransition.IsTransitioning())
			{
				Service.Get<StaRTSLogger>().Error("Transition in progress, unable to start another wipe.");
				return;
			}
			this.currentTransition = transition;
			this.currentTransition.StartWipe(planetVO);
		}

		public void ContinueWipe(IState transitionToState, IMapDataLoader mapDataLoader, TransitionCompleteDelegate onTransitionComplete)
		{
			if (!this.IsCurrentTransitionSet())
			{
				return;
			}
			this.currentTransition.ContinueWipe(transitionToState, mapDataLoader, onTransitionComplete);
		}

		public AbstractTransition GetCurrentTransition()
		{
			return this.currentTransition;
		}

		public void FinishWipe()
		{
			if (!this.IsCurrentTransitionSet())
			{
				return;
			}
			this.currentTransition.FinishWipe();
		}

		public bool IsSoftWiping()
		{
			return this.IsCurrentTransitionSet() && this.currentTransition.IsSoftWipeing();
		}

		public bool IsEverythingLoaded()
		{
			return this.IsCurrentTransitionSet() && this.currentTransition.IsEverythingLoaded();
		}

		public bool IsCurrentWorldHome()
		{
			return this.IsCurrentTransitionSet() && this.currentTransition.IsCurrentWorldHome();
		}

		public bool IsCurrentWorldUserWarBase()
		{
			return this.IsCurrentTransitionSet() && this.currentTransition.IsCurrentWorldUserWarBase();
		}

		public string GetCurrentWorldName()
		{
			if (!this.IsCurrentTransitionSet())
			{
				return string.Empty;
			}
			return this.currentTransition.GetCurrentWorldName();
		}

		public string GetCurrentWorldFactionAssetName()
		{
			if (!this.IsCurrentTransitionSet())
			{
				return string.Empty;
			}
			return this.currentTransition.GetCurrentWorldFactionAssetName();
		}

		public void SetTransitionInStartCallback(TransitionInStartDelegate startCallback)
		{
			if (!this.IsCurrentTransitionSet())
			{
				return;
			}
			this.currentTransition.SetOnTransitionInStart(startCallback);
		}

		public void SetAlertMessage(string alertMessage)
		{
			if (!this.IsCurrentTransitionSet())
			{
				return;
			}
			this.currentTransition.SetAlertMessage(alertMessage);
		}

		public void SetSkipTransitions(bool skipTransitions)
		{
			if (!this.IsCurrentTransitionSet())
			{
				return;
			}
			this.currentTransition.SetSkipTransitions(skipTransitions);
		}

		public IMapDataLoader GetMapDataLoader()
		{
			if (!this.IsCurrentTransitionSet())
			{
				return null;
			}
			return this.currentTransition.GetMapDataLoader();
		}

		public bool IsTransitioning()
		{
			return this.IsCurrentTransitionSet() && this.currentTransition.IsTransitioning();
		}

		private bool IsCurrentTransitionSet()
		{
			if (this.currentTransition == null)
			{
				Service.Get<StaRTSLogger>().Error("WorldTransitioner.currentTransition is not set.");
				return false;
			}
			return true;
		}

		protected internal WorldTransitioner(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((WorldTransitioner)GCHandledObjects.GCHandleToObject(instance)).ContinueWipe((IState)GCHandledObjects.GCHandleToObject(*args), (IMapDataLoader)GCHandledObjects.GCHandleToObject(args[1]), (TransitionCompleteDelegate)GCHandledObjects.GCHandleToObject(args[2]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((WorldTransitioner)GCHandledObjects.GCHandleToObject(instance)).FinishWipe();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldTransitioner)GCHandledObjects.GCHandleToObject(instance)).GetCurrentTransition());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldTransitioner)GCHandledObjects.GCHandleToObject(instance)).GetCurrentWorldFactionAssetName());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldTransitioner)GCHandledObjects.GCHandleToObject(instance)).GetCurrentWorldName());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldTransitioner)GCHandledObjects.GCHandleToObject(instance)).GetMapDataLoader());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldTransitioner)GCHandledObjects.GCHandleToObject(instance)).IsCurrentTransitionSet());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldTransitioner)GCHandledObjects.GCHandleToObject(instance)).IsCurrentWorldHome());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldTransitioner)GCHandledObjects.GCHandleToObject(instance)).IsCurrentWorldUserWarBase());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldTransitioner)GCHandledObjects.GCHandleToObject(instance)).IsEverythingLoaded());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldTransitioner)GCHandledObjects.GCHandleToObject(instance)).IsSoftWiping());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((WorldTransitioner)GCHandledObjects.GCHandleToObject(instance)).IsTransitioning());
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((WorldTransitioner)GCHandledObjects.GCHandleToObject(instance)).SetAlertMessage(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((WorldTransitioner)GCHandledObjects.GCHandleToObject(instance)).SetSkipTransitions(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((WorldTransitioner)GCHandledObjects.GCHandleToObject(instance)).SetTransitionInStartCallback((TransitionInStartDelegate)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((WorldTransitioner)GCHandledObjects.GCHandleToObject(instance)).StartTransition((AbstractTransition)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((WorldTransitioner)GCHandledObjects.GCHandleToObject(instance)).StartWipe((AbstractTransition)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((WorldTransitioner)GCHandledObjects.GCHandleToObject(instance)).StartWipe((AbstractTransition)GCHandledObjects.GCHandleToObject(*args), (PlanetVO)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
