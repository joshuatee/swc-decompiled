using StaRTS.Main.Models.Commands.Tournament;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1675 : $UnityType
	{
		public unsafe $UnityType1675()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 665612) = ldftn($Invoke0);
			*(data + 665640) = ldftn($Invoke1);
			*(data + 665668) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new TournamentRankRequest((UIntPtr)0);
		}
	}
}
