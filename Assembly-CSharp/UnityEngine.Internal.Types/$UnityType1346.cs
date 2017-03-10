using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1346 : $UnityType
	{
		public unsafe $UnityType1346()
		{
			*(UnityEngine.Internal.$Metadata.data + 650716) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CheatSetEquipmentRequest((UIntPtr)0);
		}
	}
}
