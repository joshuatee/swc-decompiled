using StaRTS.Main.Views;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2141 : $UnityType
	{
		public unsafe $UnityType2141()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 859904) = ldftn($Invoke0);
			*(data + 859932) = ldftn($Invoke1);
			*(data + 859960) = ldftn($Invoke2);
			*(data + 859988) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new PvpCountdownView((UIntPtr)0);
		}
	}
}
