using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class TransitionToHomeStoryAction : AbstractStoryAction
	{
		public TransitionToHomeStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(0);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			if (Service.Get<GameStateMachine>().CurrentState is HomeState)
			{
				Service.Get<StaRTSLogger>().Warn("TransitionToHomeStoryAction executed but already home.");
				this.OnTransitionComplete();
				return;
			}
			HomeState.GoToHomeState(new TransitionCompleteDelegate(this.OnTransitionComplete), false);
		}

		private void OnTransitionComplete()
		{
			this.parent.ChildComplete(this);
		}

		protected internal TransitionToHomeStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TransitionToHomeStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TransitionToHomeStoryAction)GCHandledObjects.GCHandleToObject(instance)).OnTransitionComplete();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TransitionToHomeStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
