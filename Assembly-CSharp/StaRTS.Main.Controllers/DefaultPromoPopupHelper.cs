using StaRTS.Main.Views.UX.Screens;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class DefaultPromoPopupHelper : IPromoPopupHelper
	{
		public DefaultPromoPopupHelper()
		{
		}

		public void ShowPromoDialog(OnScreenModalResult modalResult, object result)
		{
			if (modalResult != null)
			{
				modalResult(result, null);
			}
		}

		protected internal DefaultPromoPopupHelper(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DefaultPromoPopupHelper)GCHandledObjects.GCHandleToObject(instance)).ShowPromoDialog((OnScreenModalResult)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
