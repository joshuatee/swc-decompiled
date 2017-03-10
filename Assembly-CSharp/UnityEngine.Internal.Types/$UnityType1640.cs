using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1640 : $UnityType
	{
		public unsafe $UnityType1640()
		{
			*(UnityEngine.Internal.$Metadata.data + 662336) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new SquadWarAttackPlayerStartRequest((UIntPtr)0);
		}
	}
}
