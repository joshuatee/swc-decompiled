using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType933 : $UnityType
	{
		public unsafe $UnityType933()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 596872) = ldftn($Invoke0);
			*(data + 596900) = ldftn($Invoke1);
			*(data + 596928) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new PlayerLoginStartupTask((UIntPtr)0);
		}
	}
}
