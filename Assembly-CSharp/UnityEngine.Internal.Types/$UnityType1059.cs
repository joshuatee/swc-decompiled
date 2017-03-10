using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1059 : $UnityType
	{
		public unsafe $UnityType1059()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 626468) = ldftn($Invoke0);
			*(data + 626496) = ldftn($Invoke1);
			*(data + 626524) = ldftn($Invoke2);
			*(data + 626552) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new SequencePair((UIntPtr)0);
		}
	}
}
