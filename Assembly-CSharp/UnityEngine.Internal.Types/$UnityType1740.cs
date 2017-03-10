using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1740 : $UnityType
	{
		public unsafe $UnityType1740()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 675496) = ldftn($Invoke0);
			*(data + 675524) = ldftn($Invoke1);
			*(data + 675552) = ldftn($Invoke2);
			*(data + 675580) = ldftn($Invoke3);
			*(data + 675608) = ldftn($Invoke4);
			*(data + 675636) = ldftn($Invoke5);
			*(data + 675664) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new TrapViewComponent((UIntPtr)0);
		}
	}
}
