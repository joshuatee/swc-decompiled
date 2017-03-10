using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType939 : $UnityType
	{
		public unsafe $UnityType939()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 597712) = ldftn($Invoke0);
			*(data + 597740) = ldftn($Invoke1);
			*(data + 597768) = ldftn($Invoke2);
			*(data + 597796) = ldftn($Invoke3);
			*(data + 597824) = ldftn($Invoke4);
			*(data + 597852) = ldftn($Invoke5);
			*(data + 597880) = ldftn($Invoke6);
			*(data + 597908) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new StartupTask((UIntPtr)0);
		}
	}
}
