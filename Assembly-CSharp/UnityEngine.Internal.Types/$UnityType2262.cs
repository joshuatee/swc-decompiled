using StaRTS.Main.Views.UX.Elements;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2262 : $UnityType
	{
		public unsafe $UnityType2262()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 902296) = ldftn($Invoke0);
			*(data + 902324) = ldftn($Invoke1);
			*(data + 902352) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new UXScrollSpriteHandler((UIntPtr)0);
		}
	}
}
