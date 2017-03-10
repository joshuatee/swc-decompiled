using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType940 : $UnityType
	{
		public unsafe $UnityType940()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 597936) = ldftn($Invoke0);
			*(data + 597964) = ldftn($Invoke1);
			*(data + 597992) = ldftn($Invoke2);
			*(data + 598020) = ldftn($Invoke3);
			*(data + 598048) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new StartupTaskController((UIntPtr)0);
		}
	}
}
