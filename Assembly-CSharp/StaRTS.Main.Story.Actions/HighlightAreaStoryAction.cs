using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class HighlightAreaStoryAction : AbstractStoryAction
	{
		private const int GRID_ARG = 0;

		private const int BUTTON_ARG = 1;

		private const int BUFFER_WIDTH_ARG = 2;

		private const int BUFFER_HEIGHT_ARG = 3;

		private UXGrid grid;

		private UXButton dismissButton;

		public HighlightAreaStoryAction(StoryActionVO vo, IStoryReactor parent) : base(vo, parent)
		{
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(4);
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			string elementName = this.prepareArgs[0];
			string elementName2 = this.prepareArgs[1];
			int num;
			int.TryParse(this.prepareArgs[2], ref num);
			int num2;
			int.TryParse(this.prepareArgs[3], ref num2);
			this.dismissButton = Service.Get<ScreenController>().FindElement<UXButton>(elementName2);
			if (this.dismissButton == null)
			{
				this.parent.ChildComplete(this);
				return;
			}
			this.grid = Service.Get<ScreenController>().FindElement<UXGrid>(elementName);
			if (this.grid == null)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("The StoryAction {0} has an incorrect number of arguments: {1}, wrong name used.story action requires grid.", new object[]
				{
					this.vo.Uid,
					this.vo.PrepareString
				});
				this.parent.ChildComplete(this);
				return;
			}
			UserInputInhibitor userInputInhibitor = Service.Get<UserInputInhibitor>();
			userInputInhibitor.DenyAll();
			List<UXElement> list = new List<UXElement>();
			list.AddRange(this.grid.GetElementList());
			list.Add(this.dismissButton);
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				userInputInhibitor.AddToAllow(list[i]);
				i++;
			}
			NavigationCenterUpgradeScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<NavigationCenterUpgradeScreen>();
			if (highestLevelScreen != null)
			{
				highestLevelScreen.ActivateHighlight();
			}
		}

		public override void Destroy()
		{
			Service.Get<UXController>().MiscElementsManager.HideHighlight();
			NavigationCenterUpgradeScreen highestLevelScreen = Service.Get<ScreenController>().GetHighestLevelScreen<NavigationCenterUpgradeScreen>();
			if (highestLevelScreen != null)
			{
				highestLevelScreen.DeactivateHighlight();
			}
			base.Destroy();
		}

		protected internal HighlightAreaStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((HighlightAreaStoryAction)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((HighlightAreaStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((HighlightAreaStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
