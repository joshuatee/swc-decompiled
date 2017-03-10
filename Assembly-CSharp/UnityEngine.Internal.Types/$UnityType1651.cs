using StaRTS.Main.Models.Commands.Squads.Responses;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1651 : $UnityType
	{
		public unsafe $UnityType1651()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 663876) = ldftn($Invoke0);
			*(data + 663904) = ldftn($Invoke1);
			*(data + 663932) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new SquadMemberResponse((UIntPtr)0);
		}
	}
}
