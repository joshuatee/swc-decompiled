using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType973 : $UnityType
	{
		public unsafe $UnityType973()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 601492) = ldftn($Invoke0);
			*(data + 601520) = ldftn($Invoke1);
			*(data + 601548) = ldftn($Invoke2);
			*(data + 601576) = ldftn($Invoke3);
			*(data + 601604) = ldftn($Invoke4);
			*(data + 601632) = ldftn($Invoke5);
			*(data + 601660) = ldftn($Invoke6);
			*(data + 601688) = ldftn($Invoke7);
			*(data + 601716) = ldftn($Invoke8);
			*(data + 601744) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new VictoryConditionController((UIntPtr)0);
		}
	}
}
