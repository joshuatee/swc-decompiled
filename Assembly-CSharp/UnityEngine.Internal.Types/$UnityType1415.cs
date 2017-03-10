using StaRTS.Main.Models.Commands.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1415 : $UnityType
	{
		public unsafe $UnityType1415()
		{
			*(UnityEngine.Internal.$Metadata.data + 653040) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new GetObjectivesCommand((UIntPtr)0);
		}
	}
}
