using StaRTS.Main.Views.UX.Elements;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2261 : $UnityType
	{
		public unsafe $UnityType2261()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 902072) = ldftn($Invoke0);
			*(data + 902100) = ldftn($Invoke1);
			*(data + 902128) = ldftn($Invoke2);
			*(data + 902156) = ldftn($Invoke3);
			*(data + 902184) = ldftn($Invoke4);
			*(data + 902212) = ldftn($Invoke5);
			*(data + 902240) = ldftn($Invoke6);
			*(data + 902268) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new UXMeshRenderer((UIntPtr)0);
		}
	}
}
