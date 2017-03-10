using StaRTS.Main.Models.Player.Misc;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1824 : $UnityType
	{
		public unsafe $UnityType1824()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 697028) = ldftn($Invoke0);
			*(data + 697056) = ldftn($Invoke1);
			*(data + 697084) = ldftn($Invoke2);
			*(data + 697112) = ldftn($Invoke3);
			*(data + 697140) = ldftn($Invoke4);
			*(data + 697168) = ldftn($Invoke5);
			*(data + 697196) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new Campaign((UIntPtr)0);
		}
	}
}
