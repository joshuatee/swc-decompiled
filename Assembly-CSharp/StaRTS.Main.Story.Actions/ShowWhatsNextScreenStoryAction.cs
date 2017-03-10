using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class ShowWhatsNextScreenStoryAction : AbstractStoryAction, IEventObserver
	{
		public ShowWhatsNextScreenStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
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
			if (Service.Get<ScreenController>().GetHighestLevelScreen<WhatsNextFUEScreen>() == null)
			{
				Service.Get<EventManager>().RegisterObserver(this, EventId.ScreenLoaded, EventPriority.Default);
				Service.Get<ScreenController>().AddScreen(new WhatsNextFUEScreen());
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ScreenLoaded && cookie is WhatsNextFUEScreen)
			{
				Service.Get<EventManager>().UnregisterObserver(this, EventId.ScreenLoaded);
				this.parent.ChildComplete(this);
			}
			return EatResponse.NotEaten;
		}

		protected internal ShowWhatsNextScreenStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ShowWhatsNextScreenStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShowWhatsNextScreenStoryAction)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ShowWhatsNextScreenStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
