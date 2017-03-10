using StaRTS.Main.Models.Commands.Tournament;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1676 : $UnityType
	{
		public unsafe $UnityType1676()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 665696) = ldftn($Invoke0);
			*(data + 665724) = ldftn($Invoke1);
			*(data + 665752) = ldftn($Invoke2);
			*(data + 665780) = ldftn($Invoke3);
			*(data + 665808) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new TournamentRankResponse((UIntPtr)0);
		}
	}
}
