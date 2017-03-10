using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType964 : $UnityType
	{
		public unsafe $UnityType964()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 600232) = ldftn($Invoke0);
			*(data + 600260) = ldftn($Invoke1);
			*(data + 600288) = ldftn($Invoke2);
			*(data + 600316) = ldftn($Invoke3);
			*(data + 600344) = ldftn($Invoke4);
			*(data + 600372) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new DestroyUnitUidCondition((UIntPtr)0);
		}
	}
}
