using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1630 : $UnityType
	{
		public unsafe $UnityType1630()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 661496) = ldftn($Invoke0);
			*(data + 661524) = ldftn($Invoke1);
			*(data + 661552) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new GetSquadNotifsRequest((UIntPtr)0);
		}
	}
}
