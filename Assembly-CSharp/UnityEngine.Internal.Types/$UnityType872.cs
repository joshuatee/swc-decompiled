using StaRTS.Main.Controllers.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType872 : $UnityType
	{
		public unsafe $UnityType872()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 581836) = ldftn($Invoke0);
			*(data + 581864) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DonateTroopObjectiveProcessor((UIntPtr)0);
		}
	}
}
