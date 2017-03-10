using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1960 : $UnityType
	{
		public unsafe $UnityType1960()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 789036) = ldftn($Invoke0);
			*(data + 789064) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new SkinnedTroopShooterFacade((UIntPtr)0);
		}
	}
}
