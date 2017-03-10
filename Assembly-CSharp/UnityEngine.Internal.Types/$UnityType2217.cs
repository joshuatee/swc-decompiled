using StaRTS.Main.Views.UX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2217 : $UnityType
	{
		public unsafe $UnityType2217()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 880204) = ldftn($Invoke0);
			*(data + 880232) = ldftn($Invoke1);
			*(data + 880260) = ldftn($Invoke2);
			*(data + 880288) = ldftn($Invoke3);
			*(data + 880316) = ldftn($Invoke4);
			*(data + 880344) = ldftn($Invoke5);
			*(data + 880372) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new HUDResourceView((UIntPtr)0);
		}
	}
}
