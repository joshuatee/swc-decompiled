using StaRTS.Main.Models.Commands.Squads.Responses;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1648 : $UnityType
	{
		public unsafe $UnityType1648()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 663008) = ldftn($Invoke0);
			*(data + 663036) = ldftn($Invoke1);
			*(data + 663064) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new GetSquadInvitesSentResponse((UIntPtr)0);
		}
	}
}
