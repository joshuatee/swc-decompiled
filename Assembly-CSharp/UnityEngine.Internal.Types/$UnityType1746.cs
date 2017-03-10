using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1746 : $UnityType
	{
		public unsafe $UnityType1746()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 676532) = ldftn($Invoke0);
			*(data + 676560) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new TurretShooterComponent((UIntPtr)0);
		}
	}
}
