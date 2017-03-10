using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1642 : $UnityType
	{
		public unsafe $UnityType1642()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 662392) = ldftn($Invoke0);
			*(data + 662420) = ldftn($Invoke1);
			*(data + 662448) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new SquadWarParticipantIdRequest((UIntPtr)0);
		}
	}
}
