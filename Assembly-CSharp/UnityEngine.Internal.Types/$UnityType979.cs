using StaRTS.Main.Controllers.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType979 : $UnityType
	{
		public unsafe $UnityType979()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 602304) = ldftn($Invoke0);
			*(data + 602332) = ldftn($Invoke1);
			*(data + 602360) = ldftn($Invoke2);
			*(data + 602388) = ldftn($Invoke3);
			*(data + 602416) = ldftn($Invoke4);
			*(data + 602444) = ldftn($Invoke5);
			*(data + 602472) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new NeighborMapDataLoader((UIntPtr)0);
		}
	}
}
