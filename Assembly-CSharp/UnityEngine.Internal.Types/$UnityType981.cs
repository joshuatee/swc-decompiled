using StaRTS.Main.Controllers.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType981 : $UnityType
	{
		public unsafe $UnityType981()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 602836) = ldftn($Invoke0);
			*(data + 602864) = ldftn($Invoke1);
			*(data + 602892) = ldftn($Invoke2);
			*(data + 602920) = ldftn($Invoke3);
			*(data + 602948) = ldftn($Invoke4);
			*(data + 602976) = ldftn($Invoke5);
			*(data + 603004) = ldftn($Invoke6);
			*(data + 603032) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new PvpMapDataLoader((UIntPtr)0);
		}
	}
}
