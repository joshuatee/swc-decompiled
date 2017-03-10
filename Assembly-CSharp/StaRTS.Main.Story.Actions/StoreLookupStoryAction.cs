using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class StoreLookupStoryAction : AbstractStoryAction, IEventObserver
	{
		private const int TAB_ARG = 0;

		private const int ITEM_ARG = 1;

		private StoreTab tab;

		public StoreLookupStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(2);
			this.tab = StringUtils.ParseEnum<StoreTab>(this.prepareArgs[0]);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			StoreScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<StoreScreen>();
			if (highestLevelScreen != null && highestLevelScreen.IsLoaded())
			{
				Service.Get<EventManager>().RegisterObserver(this, EventId.StoreScreenReady, EventPriority.Default);
				this.PerformStoreLookup(highestLevelScreen);
				return;
			}
			Service.Get<EventManager>().RegisterObserver(this, EventId.ScreenLoaded, EventPriority.Default);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id != EventId.StoreScreenReady)
			{
				if (id == EventId.ScreenLoaded && cookie is StoreScreen)
				{
					Service.Get<EventManager>().RegisterObserver(this, EventId.StoreScreenReady, EventPriority.Default);
					this.PerformStoreLookup(cookie as StoreScreen);
				}
			}
			else
			{
				this.RemoveListeners();
				this.parent.ChildComplete(this);
			}
			return EatResponse.NotEaten;
		}

		private void PerformStoreLookup(StoreScreen store)
		{
			store.SetTab(this.tab);
			store.ScrollToItem(this.prepareArgs[1]);
			store.EnableScrollListMovement(false);
		}

		private void RemoveListeners()
		{
			Service.Get<EventManager>().UnregisterObserver(this, EventId.ScreenLoaded);
			Service.Get<EventManager>().UnregisterObserver(this, EventId.StoreScreenReady);
		}

		protected internal StoreLookupStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((StoreLookupStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StoreLookupStoryAction)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((StoreLookupStoryAction)GCHandledObjects.GCHandleToObject(instance)).PerformStoreLookup((StoreScreen)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((StoreLookupStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((StoreLookupStoryAction)GCHandledObjects.GCHandleToObject(instance)).RemoveListeners();
			return -1L;
		}
	}
}
