using StaRTS.Main.Controllers.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType986 : $UnityType
	{
		public unsafe $UnityType986()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 603592) = ldftn($Invoke0);
			*(data + 603620) = ldftn($Invoke1);
			*(data + 603648) = ldftn($Invoke2);
			*(data + 603676) = ldftn($Invoke3);
			*(data + 603704) = ldftn($Invoke4);
			*(data + 603732) = ldftn($Invoke5);
			*(data + 603760) = ldftn($Invoke6);
			*(data + 603788) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new WarBaseMapDataLoader((UIntPtr)0);
		}
	}
}
