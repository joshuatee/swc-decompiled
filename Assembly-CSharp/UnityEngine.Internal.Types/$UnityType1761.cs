using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1761 : $UnityType
	{
		public unsafe $UnityType1761()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 678072) = ldftn($Invoke0);
			*(data + 678100) = ldftn($Invoke1);
			*(data + 678128) = ldftn($Invoke2);
			*(data + 678156) = ldftn($Invoke3);
			*(data + 678184) = ldftn($Invoke4);
			*(data + 678212) = ldftn($Invoke5);
			*(data + 678240) = ldftn($Invoke6);
			*(data + 678268) = ldftn($Invoke7);
			*(data + 678296) = ldftn($Invoke8);
			*(data + 678324) = ldftn($Invoke9);
			*(data + 678352) = ldftn($Invoke10);
			*(data + 678380) = ldftn($Invoke11);
			*(data + 678408) = ldftn($Invoke12);
			*(data + 678436) = ldftn($Invoke13);
			*(data + 678464) = ldftn($Invoke14);
			*(data + 678492) = ldftn($Invoke15);
			*(data + 678520) = ldftn($Invoke16);
			*(data + 678548) = ldftn($Invoke17);
		}

		public override object CreateInstance()
		{
			return new DefensiveHealerNode((UIntPtr)0);
		}
	}
}
