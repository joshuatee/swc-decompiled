using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType924 : $UnityType
	{
		public unsafe $UnityType924()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 596340) = ldftn($Invoke0);
			*(data + 596368) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new EndpointStartupTask((UIntPtr)0);
		}
	}
}
