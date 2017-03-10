using StaRTS.Main.Views.Entities;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2181 : $UnityType
	{
		public unsafe $UnityType2181()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 868388) = ldftn($Invoke0);
			*(data + 868416) = ldftn($Invoke1);
			*(data + 868444) = ldftn($Invoke2);
			*(data + 868472) = ldftn($Invoke3);
			*(data + 868500) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new OutlinedAsset((UIntPtr)0);
		}
	}
}
