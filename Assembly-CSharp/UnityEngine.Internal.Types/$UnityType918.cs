using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType918 : $UnityType
	{
		public unsafe $UnityType918()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 596144) = ldftn($Invoke0);
			*(data + 596172) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new AudioStartupTask((UIntPtr)0);
		}
	}
}
