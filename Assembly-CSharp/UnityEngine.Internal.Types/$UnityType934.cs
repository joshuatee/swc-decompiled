using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType934 : $UnityType
	{
		public unsafe $UnityType934()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 596956) = ldftn($Invoke0);
			*(data + 596984) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new PlayerSquadStartupTask((UIntPtr)0);
		}
	}
}
