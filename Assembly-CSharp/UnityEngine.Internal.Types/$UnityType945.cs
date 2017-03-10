using StaRTS.Main.Controllers.TrapConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType945 : $UnityType
	{
		public unsafe $UnityType945()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 598300) = ldftn($Invoke0);
			*(data + 598328) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ArmorNotTrapCondition((UIntPtr)0);
		}
	}
}
