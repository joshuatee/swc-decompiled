using StaRTS.Main.Controllers.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType865 : $UnityType
	{
		public unsafe $UnityType865()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 581444) = ldftn($Invoke0);
			*(data + 581472) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DeploySpecialAttackIdObjectiveProcessor((UIntPtr)0);
		}
	}
}
