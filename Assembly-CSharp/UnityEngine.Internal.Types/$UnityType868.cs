using StaRTS.Main.Controllers.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType868 : $UnityType
	{
		public unsafe $UnityType868()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 581612) = ldftn($Invoke0);
			*(data + 581640) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DeployTroopTypeObjectiveProcessor((UIntPtr)0);
		}
	}
}
