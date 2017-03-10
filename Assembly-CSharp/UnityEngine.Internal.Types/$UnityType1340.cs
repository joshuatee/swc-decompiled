using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1340 : $UnityType
	{
		public unsafe $UnityType1340()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 650492) = ldftn($Invoke0);
			*(data + 650520) = ldftn($Invoke1);
			*(data + 650548) = ldftn($Invoke2);
			*(data + 650576) = ldftn($Invoke3);
			*(data + 650604) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new CheatScheduleDailyCrateRequest((UIntPtr)0);
		}
	}
}
