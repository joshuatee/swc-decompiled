using StaRTS.Main.Models.Battle.Replay;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1102 : $UnityType
	{
		public unsafe $UnityType1102()
		{
			*(UnityEngine.Internal.$Metadata.data + 640076) = ldftn($Invoke0);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IBattleAction)GCHandledObjects.GCHandleToObject(instance)).ActionId);
		}
	}
}
