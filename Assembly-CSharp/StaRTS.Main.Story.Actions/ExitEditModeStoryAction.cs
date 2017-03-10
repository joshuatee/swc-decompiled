using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class ExitEditModeStoryAction : AbstractStoryAction
	{
		public ExitEditModeStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
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
			if (Service.Get<GameStateMachine>().CurrentState is EditBaseState)
			{
				HomeState.GoToHomeState(null, false);
			}
			else
			{
				Service.Get<StaRTSLogger>().WarnFormat("Story Action {0} is attempting to exit Edit mode, which we are not in", new object[]
				{
					this.vo.Uid
				});
			}
			this.parent.ChildComplete(this);
		}

		protected internal ExitEditModeStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ExitEditModeStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ExitEditModeStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
