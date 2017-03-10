using StaRTS.Main.Controllers.GameStates;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType835 : $UnityType
	{
		public unsafe $UnityType835()
		{
			*(UnityEngine.Internal.$Metadata.data + 574332) = ldftn($Invoke0);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IGameState)GCHandledObjects.GCHandleToObject(instance)).CanUpdateHomeContracts());
		}
	}
}
