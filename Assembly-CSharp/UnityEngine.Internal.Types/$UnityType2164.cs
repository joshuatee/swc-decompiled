using StaRTS.Main.Views.Cameras;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2164 : $UnityType
	{
		public unsafe $UnityType2164()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 865588) = ldftn($Invoke0);
			*(data + 865616) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new UXSceneCamera((UIntPtr)0);
		}
	}
}
