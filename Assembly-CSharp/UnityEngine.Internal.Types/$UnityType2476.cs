using StaRTS.Main.Views.UX.Tags;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2476 : $UnityType
	{
		public unsafe $UnityType2476()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 971176) = ldftn($Invoke0);
			*(data + 971204) = ldftn($Invoke1);
			*(data + 971232) = ldftn($Invoke2);
			*(data + 971260) = ldftn($Invoke3);
			*(data + 971288) = ldftn($Invoke4);
			*(data + 971316) = ldftn($Invoke5);
			*(data + 971344) = ldftn($Invoke6);
			*(data + 971372) = ldftn($Invoke7);
			*(data + 971400) = ldftn($Invoke8);
			*(data + 971428) = ldftn($Invoke9);
			*(data + 971456) = ldftn($Invoke10);
			*(data + 971484) = ldftn($Invoke11);
			*(data + 971512) = ldftn($Invoke12);
			*(data + 971540) = ldftn($Invoke13);
		}

		public override object CreateInstance()
		{
			return new PrizeInventoryItemTag((UIntPtr)0);
		}
	}
}
