using StaRTS.Main.Models.Commands.Squads.Responses;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1650 : $UnityType
	{
		public unsafe $UnityType1650()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 663736) = ldftn($Invoke0);
			*(data + 663764) = ldftn($Invoke1);
			*(data + 663792) = ldftn($Invoke2);
			*(data + 663820) = ldftn($Invoke3);
			*(data + 663848) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new LeaderboardResponse((UIntPtr)0);
		}
	}
}
