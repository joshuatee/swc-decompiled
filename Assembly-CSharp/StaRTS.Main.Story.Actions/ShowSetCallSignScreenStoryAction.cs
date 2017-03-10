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
	public class ShowSetCallSignScreenStoryAction : AbstractStoryAction, IEventObserver
	{
		private bool doBackendAuth;

		public ShowSetCallSignScreenStoryAction(bool doBackendAuth, StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
			this.doBackendAuth = doBackendAuth;
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(0);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			Service.Get<EventManager>().RegisterObserver(this, EventId.ScreenLoaded, EventPriority.Default);
			if (Service.Get<ScreenController>().GetHighestLevelScreen<SetCallsignScreen>() == null)
			{
				Service.Get<ScreenController>().AddScreen(new SetCallsignScreen(this.doBackendAuth));
				return;
			}
			this.parent.ChildComplete(this);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ScreenLoaded && cookie is SetCallsignScreen)
			{
				Service.Get<EventManager>().UnregisterObserver(this, EventId.ScreenLoaded);
				this.parent.ChildComplete(this);
			}
			return EatResponse.NotEaten;
		}

		protected internal ShowSetCallSignScreenStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ShowSetCallSignScreenStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShowSetCallSignScreenStoryAction)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ShowSetCallSignScreenStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
