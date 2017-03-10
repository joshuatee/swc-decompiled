using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1792 : $UnityType
	{
		public unsafe $UnityType1792()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 684260) = ldftn($Invoke0);
			*(data + 684288) = ldftn($Invoke1);
			*(data + 684316) = ldftn($Invoke2);
			*(data + 684344) = ldftn($Invoke3);
			*(data + 684372) = ldftn($Invoke4);
			*(data + 684400) = ldftn($Invoke5);
			*(data + 684428) = ldftn($Invoke6);
			*(data + 684456) = ldftn($Invoke7);
			*(data + 684484) = ldftn($Invoke8);
			*(data + 684512) = ldftn($Invoke9);
			*(data + 684540) = ldftn($Invoke10);
			*(data + 684568) = ldftn($Invoke11);
			*(data + 684596) = ldftn($Invoke12);
			*(data + 684624) = ldftn($Invoke13);
			*(data + 684652) = ldftn($Invoke14);
			*(data + 684680) = ldftn($Invoke15);
		}

		public override object CreateInstance()
		{
			return new TroopNode((UIntPtr)0);
		}
	}
}
