using StaRTS.Main.Models.Commands.Squads.Responses;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1654 : $UnityType
	{
		public unsafe $UnityType1654()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 664296) = ldftn($Invoke0);
			*(data + 664324) = ldftn($Invoke1);
			*(data + 664352) = ldftn($Invoke2);
			*(data + 664380) = ldftn($Invoke3);
			*(data + 664408) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new SquadResponse((UIntPtr)0);
		}
	}
}
