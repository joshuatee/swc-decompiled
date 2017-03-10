using StaRTS.Main.Views.Cameras;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2159 : $UnityType
	{
		public unsafe $UnityType2159()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 863432) = ldftn($Invoke0);
			*(data + 863460) = ldftn($Invoke1);
			*(data + 863488) = ldftn($Invoke2);
			*(data + 863516) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new CameraShake((UIntPtr)0);
		}
	}
}
