using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class DisplayButtonStoryAction : AbstractStoryAction, IEventObserver
	{
		private const int BUTTON_NAME_ARG = 0;

		private const string NEXT_BUTTON = "next";

		private const string STORE_BUTTON = "store";

		public DisplayButtonStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
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
			EventManager eventManager = Service.Get<EventManager>();
			string text = this.prepareArgs[0];
			if (text == "next")
			{
				eventManager.RegisterObserver(this, EventId.StoryNextButtonClicked, EventPriority.Default);
				eventManager.SendEvent(EventId.ShowNextButton, this);
				return;
			}
			if (text == "store")
			{
				eventManager.RegisterObserver(this, EventId.StoryNextButtonClicked, EventPriority.Default);
				eventManager.SendEvent(EventId.ShowStoreNextButton, this);
				return;
			}
			Service.Get<StaRTSLogger>().ErrorFormat("No button with id {0} for DisplayButtonStoryAction!", new object[]
			{
				this.prepareArgs[0]
			});
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.StoryNextButtonClicked)
			{
				Service.Get<EventManager>().UnregisterObserver(this, EventId.StoryNextButtonClicked);
			}
			Service.Get<ViewTimerManager>().CreateViewTimer(0.15f, false, new TimerDelegate(this.CompleteActionAfterFrameDelay), null);
			return EatResponse.NotEaten;
		}

		private void CompleteActionAfterFrameDelay(uint id, object cookie)
		{
			this.parent.ChildComplete(this);
		}

		protected internal DisplayButtonStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DisplayButtonStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DisplayButtonStoryAction)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((DisplayButtonStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
