using StaRTS.Main.Views.Cameras;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2162 : $UnityType
	{
		public unsafe $UnityType2162()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 865028) = ldftn($Invoke0);
			*(data + 865056) = ldftn($Invoke1);
			*(data + 865084) = ldftn($Invoke2);
			*(data + 865112) = ldftn($Invoke3);
			*(data + 865140) = ldftn($Invoke4);
			*(data + 865168) = ldftn($Invoke5);
			*(data + 865196) = ldftn($Invoke6);
			*(data + 865224) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new RenderTextureItem((UIntPtr)0);
		}
	}
}
