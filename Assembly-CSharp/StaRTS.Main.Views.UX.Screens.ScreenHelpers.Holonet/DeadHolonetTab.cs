using StaRTS.Main.Controllers.Holonet;
using StaRTS.Main.Views.UX.Elements;
using System;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet
{
	public class DeadHolonetTab : AbstractHolonetTab
	{
		public DeadHolonetTab(HolonetScreen screen, HolonetControllerType type, string name) : base(screen, type)
		{
			this.topLevelGroup = screen.GetElement<UXElement>(name);
			this.topLevelGroup.Visible = false;
		}

		public override void OnTabOpen()
		{
		}

		public override void OnTabClose()
		{
		}

		protected override void AddSelectionButtonToNavTable()
		{
		}

		protected internal DeadHolonetTab(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DeadHolonetTab)GCHandledObjects.GCHandleToObject(instance)).AddSelectionButtonToNavTable();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DeadHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnTabClose();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((DeadHolonetTab)GCHandledObjects.GCHandleToObject(instance)).OnTabOpen();
			return -1L;
		}
	}
}
