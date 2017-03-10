using StaRTS.Main.Controllers.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType989 : $UnityType
	{
		public unsafe $UnityType989()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 604432) = ldftn($Invoke0);
			*(data + 604460) = ldftn($Invoke1);
			*(data + 604488) = ldftn($Invoke2);
			*(data + 604516) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new WorldPreloadAsset((UIntPtr)0);
		}
	}
}
