using StaRTS.Main.Views.Cameras;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2158 : $UnityType
	{
		public unsafe $UnityType2158()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 863152) = ldftn($Invoke0);
			*(data + 863180) = ldftn($Invoke1);
			*(data + 863208) = ldftn($Invoke2);
			*(data + 863236) = ldftn($Invoke3);
			*(data + 863264) = ldftn($Invoke4);
			*(data + 863292) = ldftn($Invoke5);
			*(data + 863320) = ldftn($Invoke6);
			*(data + 863348) = ldftn($Invoke7);
			*(data + 863376) = ldftn($Invoke8);
			*(data + 863404) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new CameraManager((UIntPtr)0);
		}
	}
}
