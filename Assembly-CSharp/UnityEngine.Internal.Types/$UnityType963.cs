using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType963 : $UnityType
	{
		public unsafe $UnityType963()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 600064) = ldftn($Invoke0);
			*(data + 600092) = ldftn($Invoke1);
			*(data + 600120) = ldftn($Invoke2);
			*(data + 600148) = ldftn($Invoke3);
			*(data + 600176) = ldftn($Invoke4);
			*(data + 600204) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new DestroyUnitTypeCondition((UIntPtr)0);
		}
	}
}
