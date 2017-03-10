using StaRTS.Main.Models.Commands.Equipment;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1398 : $UnityType
	{
		public unsafe $UnityType1398()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 652200) = ldftn($Invoke0);
			*(data + 652228) = ldftn($Invoke1);
			*(data + 652256) = ldftn($Invoke2);
			*(data + 652284) = ldftn($Invoke3);
			*(data + 652312) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new EquipmentUpgradeStartRequest((UIntPtr)0);
		}
	}
}
