using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType944 : $UnityType
	{
		public unsafe $UnityType944()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 598244) = ldftn($Invoke0);
			*(data + 598272) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new WorldStartupTask((UIntPtr)0);
		}
	}
}
