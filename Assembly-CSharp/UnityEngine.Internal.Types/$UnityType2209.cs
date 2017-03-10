using StaRTS.Main.Views.Projectors;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2209 : $UnityType
	{
		public unsafe $UnityType2209()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 872420) = ldftn($Invoke0);
			*(data + 872448) = ldftn($Invoke1);
			*(data + 872476) = ldftn($Invoke2);
			*(data + 872504) = ldftn($Invoke3);
			*(data + 872532) = ldftn($Invoke4);
			*(data + 872560) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new SpriteProjectorRenderer((UIntPtr)0);
		}
	}
}
