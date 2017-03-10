using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType747 : $UnityType
	{
		public unsafe $UnityType747()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 546948) = ldftn($Invoke0);
			*(data + 546976) = ldftn($Invoke1);
			*(data + 547004) = ldftn($Invoke2);
			*(data + 547032) = ldftn($Invoke3);
			*(data + 547060) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new QuietCorrectionController((UIntPtr)0);
		}
	}
}
