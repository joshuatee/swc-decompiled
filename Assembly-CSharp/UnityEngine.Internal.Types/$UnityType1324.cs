using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1324 : $UnityType
	{
		public unsafe $UnityType1324()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 650100) = ldftn($Invoke0);
			*(data + 650128) = ldftn($Invoke1);
			*(data + 650156) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new CheatEarnEquipmentShardRequest((UIntPtr)0);
		}
	}
}
