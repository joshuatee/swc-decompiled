using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Core;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Story.Actions
{
	public class ShowHologramInfoStoryAction : AbstractStoryAction
	{
		private const int ARG_IMG_NAME = 0;

		private const int ARG_DISPLAY_TEXT = 1;

		private const int ARG_TITLE_TEXT = 2;

		public string ImageName
		{
			get;
			private set;
		}

		public string DisplayText
		{
			get;
			private set;
		}

		public string TitleText
		{
			get;
			private set;
		}

		public bool PlanetPanel
		{
			get;
			private set;
		}

		public ShowHologramInfoStoryAction(StoryActionVO vo, IStoryReactor parent, bool planetPanel) : base(vo, parent)
		{
			this.PlanetPanel = planetPanel;
		}

		public override void Prepare()
		{
			base.VerifyArgumentCount(new int[]
			{
				1,
				2,
				3
			});
			this.ImageName = this.prepareArgs[0];
			if (this.prepareArgs.Length > 1)
			{
				this.DisplayText = this.prepareArgs[1];
			}
			if (this.prepareArgs.Length > 2)
			{
				this.TitleText = this.prepareArgs[2];
			}
			this.parent.ChildPrepared(this);
		}

		public override void Execute()
		{
			base.Execute();
			Service.Get<EventManager>().SendEvent(EventId.ShowInfoPanel, this);
			this.parent.ChildComplete(this);
		}

		protected internal ShowHologramInfoStoryAction(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ShowHologramInfoStoryAction)GCHandledObjects.GCHandleToObject(instance)).Execute();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShowHologramInfoStoryAction)GCHandledObjects.GCHandleToObject(instance)).DisplayText);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShowHologramInfoStoryAction)GCHandledObjects.GCHandleToObject(instance)).ImageName);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShowHologramInfoStoryAction)GCHandledObjects.GCHandleToObject(instance)).PlanetPanel);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShowHologramInfoStoryAction)GCHandledObjects.GCHandleToObject(instance)).TitleText);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ShowHologramInfoStoryAction)GCHandledObjects.GCHandleToObject(instance)).Prepare();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ShowHologramInfoStoryAction)GCHandledObjects.GCHandleToObject(instance)).DisplayText = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ShowHologramInfoStoryAction)GCHandledObjects.GCHandleToObject(instance)).ImageName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ShowHologramInfoStoryAction)GCHandledObjects.GCHandleToObject(instance)).PlanetPanel = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((ShowHologramInfoStoryAction)GCHandledObjects.GCHandleToObject(instance)).TitleText = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
