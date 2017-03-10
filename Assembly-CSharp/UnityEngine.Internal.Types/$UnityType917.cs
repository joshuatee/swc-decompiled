using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType917 : $UnityType
	{
		public unsafe $UnityType917()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 596088) = ldftn($Invoke0);
			*(data + 596116) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new AssetStartupTask((UIntPtr)0);
		}
	}
}
