using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType968 : $UnityType
	{
		public unsafe $UnityType968()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 600764) = ldftn($Invoke0);
			*(data + 600792) = ldftn($Invoke1);
			*(data + 600820) = ldftn($Invoke2);
			*(data + 600848) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new OwnResourceCondition((UIntPtr)0);
		}
	}
}
