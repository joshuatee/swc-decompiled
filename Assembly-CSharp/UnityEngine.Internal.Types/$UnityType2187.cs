using StaRTS.Main.Views.Entities;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2187 : $UnityType
	{
		public unsafe $UnityType2187()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 869424) = ldftn($Invoke0);
			*(data + 869452) = ldftn($Invoke1);
			*(data + 869480) = ldftn($Invoke2);
			*(data + 869508) = ldftn($Invoke3);
			*(data + 869536) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new ViewFader((UIntPtr)0);
		}
	}
}
