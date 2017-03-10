using StaRTS.Main.Views.UserInput;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2485 : $UnityType
	{
		public unsafe $UnityType2485()
		{
			*(UnityEngine.Internal.$Metadata.data + 975180) = ldftn($Invoke0);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IBackButtonHandler)GCHandledObjects.GCHandleToObject(instance)).HandleBackButtonPress());
		}
	}
}
