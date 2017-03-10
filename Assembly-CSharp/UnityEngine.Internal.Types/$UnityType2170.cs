using StaRTS.Main.Views.Entities;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2170 : $UnityType
	{
		public unsafe $UnityType2170()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 866512) = ldftn($Invoke0);
			*(data + 866540) = ldftn($Invoke1);
			*(data + 866568) = ldftn($Invoke2);
			*(data + 866596) = ldftn($Invoke3);
			*(data + 866624) = ldftn($Invoke4);
			*(data + 866652) = ldftn($Invoke5);
			*(data + 866680) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new AbstractFadingView((UIntPtr)0);
		}
	}
}
