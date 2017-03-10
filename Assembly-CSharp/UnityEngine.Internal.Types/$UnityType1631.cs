using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1631 : $UnityType
	{
		public unsafe $UnityType1631()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 661580) = ldftn($Invoke0);
			*(data + 661608) = ldftn($Invoke1);
			*(data + 661636) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new GetSquadWarStatusRequest((UIntPtr)0);
		}
	}
}
