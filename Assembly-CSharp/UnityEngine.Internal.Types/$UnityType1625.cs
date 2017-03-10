using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1625 : $UnityType
	{
		public unsafe $UnityType1625()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 660964) = ldftn($Invoke0);
			*(data + 660992) = ldftn($Invoke1);
			*(data + 661020) = ldftn($Invoke2);
			*(data + 661048) = ldftn($Invoke3);
			*(data + 661076) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new ApplyToSquadRequest((UIntPtr)0);
		}
	}
}
