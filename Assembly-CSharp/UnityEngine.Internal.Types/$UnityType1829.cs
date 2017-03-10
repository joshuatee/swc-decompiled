using StaRTS.Main.Models.Player.Misc;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1829 : $UnityType
	{
		public unsafe $UnityType1829()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 699968) = ldftn($Invoke0);
			*(data + 699996) = ldftn($Invoke1);
			*(data + 700024) = ldftn($Invoke2);
			*(data + 700052) = ldftn($Invoke3);
			*(data + 700080) = ldftn($Invoke4);
			*(data + 700108) = ldftn($Invoke5);
			*(data + 700136) = ldftn($Invoke6);
			*(data + 700164) = ldftn($Invoke7);
			*(data + 700192) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new TournamentProgress((UIntPtr)0);
		}
	}
}
