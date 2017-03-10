using StaRTS.Main.Models.Commands.Equipment;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1396 : $UnityType
	{
		public unsafe $UnityType1396()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 652116) = ldftn($Invoke0);
			*(data + 652144) = ldftn($Invoke1);
			*(data + 652172) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new EquipmentIdRequest((UIntPtr)0);
		}
	}
}
