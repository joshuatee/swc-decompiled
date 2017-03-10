using StaRTS.Main.Models.Squads.War;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1875 : $UnityType
	{
		public unsafe $UnityType1875()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 707164) = ldftn($Invoke0);
			*(data + 707192) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new SquadWarParticipantState((UIntPtr)0);
		}
	}
}
