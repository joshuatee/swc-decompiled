using StaRTS.Externals.Maker.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType453 : $UnityType
	{
		public unsafe $UnityType453()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 452336) = ldftn($Invoke0);
			*(data + 452364) = ldftn($Invoke1);
			*(data + 452392) = ldftn($Invoke2);
			*(data + 452420) = ldftn($Invoke3);
			*(data + 452448) = ldftn($Invoke4);
			*(data + 452476) = ldftn($Invoke5);
			*(data + 452504) = ldftn($Invoke6);
			*(data + 452532) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new VideoPlayerHelper((UIntPtr)0);
		}
	}
}
