using StaRTS.Externals.Maker.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType452 : $UnityType
	{
		public unsafe $UnityType452()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 452112) = ldftn($Invoke0);
			*(data + 452140) = ldftn($Invoke1);
			*(data + 452168) = ldftn($Invoke2);
			*(data + 452196) = ldftn($Invoke3);
			*(data + 452224) = ldftn($Invoke4);
			*(data + 452252) = ldftn($Invoke5);
			*(data + 452280) = ldftn($Invoke6);
			*(data + 452308) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new VideoPlayer((UIntPtr)0);
		}
	}
}
