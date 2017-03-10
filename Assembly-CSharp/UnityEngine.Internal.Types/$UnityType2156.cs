using StaRTS.Main.Views.Cameras;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2156 : $UnityType
	{
		public unsafe $UnityType2156()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 863012) = ldftn($Invoke0);
			*(data + 863040) = ldftn($Invoke1);
			*(data + 863068) = ldftn($Invoke2);
			*(data + 863096) = ldftn($Invoke3);
			*(data + 863124) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new CameraBase((UIntPtr)0);
		}
	}
}
