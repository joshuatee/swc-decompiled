using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType935 : $UnityType
	{
		public unsafe $UnityType935()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 597012) = ldftn($Invoke0);
			*(data + 597040) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new PreloadStartupTask((UIntPtr)0);
		}
	}
}
