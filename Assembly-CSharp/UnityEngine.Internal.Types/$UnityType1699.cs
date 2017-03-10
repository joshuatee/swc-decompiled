using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1699 : $UnityType
	{
		public unsafe $UnityType1699()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 668132) = ldftn($Invoke0);
			*(data + 668160) = ldftn($Invoke1);
			*(data + 668188) = ldftn($Invoke2);
			*(data + 668216) = ldftn($Invoke3);
			*(data + 668244) = ldftn($Invoke4);
			*(data + 668272) = ldftn($Invoke5);
			*(data + 668300) = ldftn($Invoke6);
			*(data + 668328) = ldftn($Invoke7);
			*(data + 668356) = ldftn($Invoke8);
			*(data + 668384) = ldftn($Invoke9);
			*(data + 668412) = ldftn($Invoke10);
			*(data + 668440) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new DefenderComponent((UIntPtr)0);
		}
	}
}
