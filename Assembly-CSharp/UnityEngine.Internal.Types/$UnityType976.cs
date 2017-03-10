using StaRTS.Main.Controllers.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType976 : $UnityType
	{
		public unsafe $UnityType976()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 602192) = ldftn($Invoke0);
			*(data + 602220) = ldftn($Invoke1);
			*(data + 602248) = ldftn($Invoke2);
			*(data + 602276) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new MapDataLoaderUtils((UIntPtr)0);
		}
	}
}
