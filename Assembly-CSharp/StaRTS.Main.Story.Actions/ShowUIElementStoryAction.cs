using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class ShowUIElementStoryAction : AbstractStoryAction
	{
		private const int ARG_ELEMENT_NAME = 0;

		private string elementName;

		private bool show;

		public ShowUIElementStoryAction(StoryActionVO vo, IStoryReactor parent, bool show) : base(vo, parent)
		{
			this.show = show;
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(1);
			this.elementName = this.prepareArgs[0];
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			UXElement uXElement = Service.Get<ScreenController>().FindElement<UXElement>(this.elementName);
			if (uXElement != null)
			{
				uXElement.Visible = this.show;
			}
			else
			{
				Service.Get<StaRTSLogger>().WarnFormat("No element of name {0} exists in the UI currently!", new object[]
				{
					this.elementName
				});
			}
			this.parent.ChildComplete(this);
		}

		protected internal ShowUIElementStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ShowUIElementStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ShowUIElementStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}
	}
}
