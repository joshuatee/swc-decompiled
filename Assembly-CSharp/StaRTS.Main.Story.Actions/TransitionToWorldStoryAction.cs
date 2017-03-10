using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.ValueObjects;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class TransitionToWorldStoryAction : AbstractStoryAction
	{
		private const int WORLD_UID_ARG = 0;

		public TransitionToWorldStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(1);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			BattleInitializationData data = BattleInitializationData.CreateFromBattleTypeVO(this.prepareArgs[0]);
			BattleStartState.GoToBattleStartState(data, new TransitionCompleteDelegate(this.OnTransitionComplete));
		}

		private void OnTransitionComplete()
		{
			this.parent.ChildComplete(this);
		}

		protected internal TransitionToWorldStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TransitionToWorldStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TransitionToWorldStoryAction)GCHandledObjects.GCHandleToObject(instance)).OnTransitionComplete();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TransitionToWorldStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
