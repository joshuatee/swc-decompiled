using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1641 : $UnityType
	{
		public unsafe $UnityType1641()
		{
			*(UnityEngine.Internal.$Metadata.data + 662364) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new SquadWarGetBuffBaseStatusRequest((UIntPtr)0);
		}
	}
}
