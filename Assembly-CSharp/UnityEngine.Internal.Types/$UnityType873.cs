using StaRTS.Main.Controllers.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType873 : $UnityType
	{
		public unsafe $UnityType873()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 581892) = ldftn($Invoke0);
			*(data + 581920) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DonateTroopTypeObjectiveProcessor((UIntPtr)0);
		}
	}
}
