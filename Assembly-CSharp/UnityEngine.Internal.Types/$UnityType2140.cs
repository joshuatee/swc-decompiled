using StaRTS.Main.Views;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2140 : $UnityType
	{
		public unsafe $UnityType2140()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 859736) = ldftn($Invoke0);
			*(data + 859764) = ldftn($Invoke1);
			*(data + 859792) = ldftn($Invoke2);
			*(data + 859820) = ldftn($Invoke3);
			*(data + 859848) = ldftn($Invoke4);
			*(data + 859876) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new LongPressView((UIntPtr)0);
		}
	}
}
