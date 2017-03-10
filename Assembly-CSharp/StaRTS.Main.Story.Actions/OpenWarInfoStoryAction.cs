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
	public class OpenWarInfoStoryAction : AbstractStoryAction, IEventObserver
	{
		private const int HELP_STATE_ARG = 0;

		private int pageIndex;

		public OpenWarInfoStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			if (this.prepareArgs.Length == 0)
			{
				this.pageIndex = -1;
			}
			else
			{
				this.pageIndex = Convert.ToInt32(this.prepareArgs[0], CultureInfo.InvariantCulture);
			}
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			ScreenController screenController = Service.Get<ScreenController>();
			SquadWarInfoScreen squadWarInfoScreen = screenController.GetHighestLevelScreen<SquadWarInfoScreen>();
			if (squadWarInfoScreen == null)
			{
				squadWarInfoScreen = new SquadWarInfoScreen(this.pageIndex);
				Service.Get<ScreenController>().AddScreen(squadWarInfoScreen);
				return;
			}
			if (squadWarInfoScreen.IsLoaded())
			{
				squadWarInfoScreen.ShowPage(this.pageIndex);
				this.parent.ChildComplete(this);
				return;
			}
			Service.Get<EventManager>().RegisterObserver(this, EventId.ScreenLoaded);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.ScreenLoaded && cookie is SquadWarInfoScreen)
			{
				Service.Get<EventManager>().UnregisterObserver(this, EventId.ScreenLoaded);
				((SquadWarInfoScreen)cookie).ShowPage(this.pageIndex);
				this.parent.ChildComplete(this);
			}
			return EatResponse.NotEaten;
		}

		protected internal OpenWarInfoStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((OpenWarInfoStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OpenWarInfoStoryAction)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((OpenWarInfoStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
