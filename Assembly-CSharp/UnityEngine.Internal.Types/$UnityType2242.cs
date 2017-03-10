using StaRTS.Main.Views.UX;
using StaRTS.Main.Views.UX.Elements;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2242 : $UnityType
	{
		public unsafe $UnityType2242()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 894512) = ldftn($Invoke0);
			*(data + 894540) = ldftn($Invoke1);
			*(data + 894568) = ldftn($Invoke2);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((IUXScrollSpriteHandler)GCHandledObjects.GCHandleToObject(instance)).HideScrollSprites();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((IUXScrollSpriteHandler)GCHandledObjects.GCHandleToObject(instance)).InitScrollSprites((UXFactory)GCHandledObjects.GCHandleToObject(*args), (UIScrollView)GCHandledObjects.GCHandleToObject(args[1]), *(float*)(args + 2), *(sbyte*)(args + 3) != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((IUXScrollSpriteHandler)GCHandledObjects.GCHandleToObject(instance)).UpdateScrollSprites((UIScrollView)GCHandledObjects.GCHandleToObject(*args), *(float*)(args + 1), *(sbyte*)(args + 2) != 0);
			return -1L;
		}
	}
}
