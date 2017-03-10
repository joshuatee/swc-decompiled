using StaRTS.Main.Controllers.TrapConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType946 : $UnityType
	{
		public unsafe $UnityType946()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 598356) = ldftn($Invoke0);
			*(data + 598384) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new IntruderTypeTrapCondition((UIntPtr)0);
		}
	}
}
