using StaRTS.Externals.Maker;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType438 : $UnityType
	{
		public unsafe $UnityType438()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 447912) = ldftn($Invoke0);
			*(data + 447940) = ldftn($Invoke1);
			*(data + 447968) = ldftn($Invoke2);
			*(data + 447996) = ldftn($Invoke3);
			*(data + 448024) = ldftn($Invoke4);
			*(data + 448052) = ldftn($Invoke5);
			*(data + 448080) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new VideosFeatured((UIntPtr)0);
		}
	}
}
