using StaRTS.Main.Controllers.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType866 : $UnityType
	{
		public unsafe $UnityType866()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 581500) = ldftn($Invoke0);
			*(data + 581528) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DeploySpecialAttackObjectiveProcessor((UIntPtr)0);
		}
	}
}
