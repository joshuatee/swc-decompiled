using StaRTS.Main.Controllers.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType867 : $UnityType
	{
		public unsafe $UnityType867()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 581556) = ldftn($Invoke0);
			*(data + 581584) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DeployTroopIdObjectiveProcessor((UIntPtr)0);
		}
	}
}
