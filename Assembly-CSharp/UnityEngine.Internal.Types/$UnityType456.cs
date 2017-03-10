using StaRTS.Externals.Manimal;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType456 : $UnityType
	{
		public unsafe $UnityType456()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 453148) = ldftn($Invoke0);
			*(data + 453176) = ldftn($Invoke1);
			*(data + 453204) = ldftn($Invoke2);
			*(data + 453232) = ldftn($Invoke3);
			*(data + 453260) = ldftn($Invoke4);
			*(data + 453288) = ldftn($Invoke5);
			*(data + 453316) = ldftn($Invoke6);
			*(data + 453344) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new Dispatcher((UIntPtr)0);
		}
	}
}
