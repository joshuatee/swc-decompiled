using StaRTS.Main.Models.Commands.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1414 : $UnityType
	{
		public unsafe $UnityType1414()
		{
			*(UnityEngine.Internal.$Metadata.data + 653012) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new ForceObjectivesUpdateCommand((UIntPtr)0);
		}
	}
}
