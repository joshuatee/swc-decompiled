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
	public class OpenStoreScreenStoryAction : AbstractStoryAction, IEventObserver
	{
		public OpenStoreScreenStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
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
			StoreScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<StoreScreen>();
			if (highestLevelScreen != null && highestLevelScreen.IsLoaded())
			{
				this.parent.ChildComplete(this);
				return;
			}
			Service.Get<EventManager>().RegisterObserver(this, EventId.ScreenLoaded, EventPriority.Default);
			if (highestLevelScreen == null)
			{
				Service.Get<ScreenController>().AddScreen(new StoreScreen());
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ScreenLoaded && cookie is StoreScreen)
			{
				Service.Get<EventManager>().UnregisterObserver(this, EventId.ScreenLoaded);
				this.parent.ChildComplete(this);
			}
			return EatResponse.NotEaten;
		}

		protected internal OpenStoreScreenStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((OpenStoreScreenStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OpenStoreScreenStoryAction)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((OpenStoreScreenStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
