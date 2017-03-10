using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1633 : $UnityType
	{
		public unsafe $UnityType1633()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 661748) = ldftn($Invoke0);
			*(data + 661776) = ldftn($Invoke1);
			*(data + 661804) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new PlayerLeaderboardRequest((UIntPtr)0);
		}
	}
}
