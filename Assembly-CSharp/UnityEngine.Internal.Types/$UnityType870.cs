using StaRTS.Main.Controllers.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType870 : $UnityType
	{
		public unsafe $UnityType870()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 581724) = ldftn($Invoke0);
			*(data + 581752) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DestroyBuildingTypeObjectiveProcessor((UIntPtr)0);
		}
	}
}
