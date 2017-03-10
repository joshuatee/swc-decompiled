using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType952 : $UnityType
	{
		public unsafe $UnityType952()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 598692) = ldftn($Invoke0);
			*(data + 598720) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ConditionFactory((UIntPtr)0);
		}
	}
}
