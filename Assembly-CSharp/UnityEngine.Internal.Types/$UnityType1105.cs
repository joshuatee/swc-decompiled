using StaRTS.Main.Models.Battle.Replay;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1105 : $UnityType
	{
		public unsafe $UnityType1105()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 640608) = ldftn($Invoke0);
			*(data + 640636) = ldftn($Invoke1);
			*(data + 640664) = ldftn($Invoke2);
			*(data + 640692) = ldftn($Invoke3);
			*(data + 640720) = ldftn($Invoke4);
			*(data + 640748) = ldftn($Invoke5);
			*(data + 640776) = ldftn($Invoke6);
			*(data + 640804) = ldftn($Invoke7);
			*(data + 640832) = ldftn($Invoke8);
			*(data + 640860) = ldftn($Invoke9);
			*(data + 640888) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new TroopPlacedAction((UIntPtr)0);
		}
	}
}
