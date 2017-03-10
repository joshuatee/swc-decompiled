using StaRTS.Externals.Manimal;
using StaRTS.Main.Models;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class IOSPromoPopupHelper : IPromoPopupHelper
	{
		public IOSPromoPopupHelper()
		{
		}

		public void ShowPromoDialog(OnScreenModalResult modalResult, object result)
		{
			int num = 0;
			string iOS_PROMO_END_DATE = GameConstants.IOS_PROMO_END_DATE;
			if (!string.IsNullOrEmpty(iOS_PROMO_END_DATE))
			{
				DateTime date = DateTime.ParseExact(iOS_PROMO_END_DATE, "HH:mm,dd-MM-yyyy", CultureInfo.InvariantCulture);
				num = DateUtils.GetSecondsFromEpoch(date);
			}
			if ((ulong)ServerTime.Time < (ulong)((long)num))
			{
				Service.Get<ScreenController>().AddScreen(new ApplePromoScreen(modalResult, result));
				return;
			}
			if (modalResult != null)
			{
				modalResult(result, null);
			}
		}

		protected internal IOSPromoPopupHelper(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IOSPromoPopupHelper)GCHandledObjects.GCHandleToObject(instance)).ShowPromoDialog((OnScreenModalResult)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
