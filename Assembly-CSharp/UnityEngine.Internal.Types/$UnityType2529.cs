using StaRTS.RuntimeTools.BigHeadMode;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2529 : $UnityType
	{
		public unsafe $UnityType2529()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 987892) = ldftn($Invoke0);
			*(data + 987920) = ldftn($Invoke1);
			*(data + 987948) = ldftn($Invoke2);
			*(data + 987976) = ldftn($Invoke3);
			*(data + 988004) = ldftn($Invoke4);
			*(data + 988032) = ldftn($Invoke5);
			*(data + 988060) = ldftn($Invoke6);
			*(data + 988088) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new BigHeadModeController((UIntPtr)0);
		}
	}
}
