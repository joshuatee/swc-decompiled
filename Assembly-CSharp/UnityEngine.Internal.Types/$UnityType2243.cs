using StaRTS.Main.Views.UX.Elements;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2243 : $UnityType
	{
		public unsafe $UnityType2243()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 894596) = ldftn($Invoke0);
			*(data + 894624) = ldftn($Invoke1);
			*(data + 894652) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new NoOpUXScrollSpriteHandler((UIntPtr)0);
		}
	}
}
