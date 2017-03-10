using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType729 : $UnityType
	{
		public unsafe $UnityType729()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 541992) = ldftn($Invoke0);
			*(data + 542020) = ldftn($Invoke1);
			*(data + 542048) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new MainController((UIntPtr)0);
		}
	}
}
