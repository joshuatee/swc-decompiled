using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType927 : $UnityType
	{
		public unsafe $UnityType927()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 596452) = ldftn($Invoke0);
			*(data + 596480) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new GeneralStartupTask((UIntPtr)0);
		}
	}
}
