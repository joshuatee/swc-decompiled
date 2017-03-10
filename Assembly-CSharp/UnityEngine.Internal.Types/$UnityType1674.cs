using StaRTS.Main.Models.Commands.Tournament;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1674 : $UnityType
	{
		public unsafe $UnityType1674()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 665472) = ldftn($Invoke0);
			*(data + 665500) = ldftn($Invoke1);
			*(data + 665528) = ldftn($Invoke2);
			*(data + 665556) = ldftn($Invoke3);
			*(data + 665584) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new TournamentRank((UIntPtr)0);
		}
	}
}
