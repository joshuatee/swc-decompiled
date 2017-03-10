using StaRTS.Main.Controllers;
using StaRTS.Main.Views.UX.Screens;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType720 : $UnityType
	{
		public unsafe $UnityType720()
		{
			*(UnityEngine.Internal.$Metadata.data + 537232) = ldftn($Invoke0);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IPromoPopupHelper)GCHandledObjects.GCHandleToObject(instance)).ShowPromoDialog((OnScreenModalResult)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
