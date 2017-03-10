using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class TrainingInstructionsStoryAction : AbstractStoryAction, IEventObserver
	{
		private const int INSTRUCTION_STRING_KEY_ARG = 0;

		private const int COUNT_KEY_ARG = 1;

		private TroopTrainingScreen tts;

		private EventManager events;

		public TrainingInstructionsStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(2);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			this.tts = Service.Get<ScreenController>().GetHighestLevelScreen<TroopTrainingScreen>();
			if (this.tts == null || !this.tts.IsLoaded())
			{
				this.events = Service.Get<EventManager>();
				this.events.RegisterObserver(this, EventId.ScreenLoaded, EventPriority.Default);
				return;
			}
			if (!this.tts.TransitionedIn)
			{
				TroopTrainingScreen expr_5C = this.tts;
				expr_5C.OnTransitionInComplete = (OnTransInComplete)Delegate.Combine(expr_5C.OnTransitionInComplete, new OnTransInComplete(this.SetSpecialInstructions));
				return;
			}
			this.SetSpecialInstructions();
		}

		private void SetSpecialInstructions()
		{
			string instructionsUid = this.prepareArgs[0];
			int maxCount = Convert.ToInt32(this.prepareArgs[1], CultureInfo.InvariantCulture);
			this.tts.DisableTroopItemScrolling();
			this.tts.DisableTabSelection();
			this.tts.SetSpecialInstructions(instructionsUid, maxCount);
			this.parent.ChildComplete(this);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ScreenLoaded && cookie is TroopTrainingScreen)
			{
				this.RemoveListeners();
				this.Execute();
			}
			return EatResponse.NotEaten;
		}

		private void RemoveListeners()
		{
			this.events.UnregisterObserver(this, EventId.ScreenLoaded);
		}

		protected internal TrainingInstructionsStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((TrainingInstructionsStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TrainingInstructionsStoryAction)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TrainingInstructionsStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TrainingInstructionsStoryAction)GCHandledObjects.GCHandleToObject(instance)).RemoveListeners();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TrainingInstructionsStoryAction)GCHandledObjects.GCHandleToObject(instance)).SetSpecialInstructions();
			return -1L;
		}
	}
}
