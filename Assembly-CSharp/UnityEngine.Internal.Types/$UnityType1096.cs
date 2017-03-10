using StaRTS.Main.Models.Battle.Replay;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1096 : $UnityType
	{
		public unsafe $UnityType1096()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 637052) = ldftn($Invoke0);
			*(data + 637080) = ldftn($Invoke1);
			*(data + 637108) = ldftn($Invoke2);
			*(data + 637136) = ldftn($Invoke3);
			*(data + 637164) = ldftn($Invoke4);
			*(data + 637192) = ldftn($Invoke5);
			*(data + 637220) = ldftn($Invoke6);
			*(data + 637248) = ldftn($Invoke7);
			*(data + 637276) = ldftn($Invoke8);
			*(data + 637304) = ldftn($Invoke9);
			*(data + 637332) = ldftn($Invoke10);
			*(data + 637360) = ldftn($Invoke11);
			*(data + 637388) = ldftn($Invoke12);
			*(data + 637416) = ldftn($Invoke13);
			*(data + 637444) = ldftn($Invoke14);
			*(data + 637472) = ldftn($Invoke15);
			*(data + 637500) = ldftn($Invoke16);
			*(data + 637528) = ldftn($Invoke17);
		}

		public override object CreateInstance()
		{
			return new BattleAttributes((UIntPtr)0);
		}
	}
}
