using StaRTS.Utils.MetaData;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2574 : $UnityType
	{
		public unsafe $UnityType2574()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 997384) = ldftn($Invoke0);
			*(data + 997412) = ldftn($Invoke1);
			*(data + 997440) = ldftn($Invoke2);
			*(data + 997468) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new Column((UIntPtr)0);
		}
	}
}
