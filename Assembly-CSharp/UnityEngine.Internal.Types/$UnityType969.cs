using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType969 : $UnityType
	{
		public unsafe $UnityType969()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 600876) = ldftn($Invoke0);
			*(data + 600904) = ldftn($Invoke1);
			*(data + 600932) = ldftn($Invoke2);
			*(data + 600960) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new OwnUnitCondition((UIntPtr)0);
		}
	}
}
