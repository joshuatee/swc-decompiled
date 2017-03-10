using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1646 : $UnityType
	{
		public unsafe $UnityType1646()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 662784) = ldftn($Invoke0);
			*(data + 662812) = ldftn($Invoke1);
			*(data + 662840) = ldftn($Invoke2);
			*(data + 662868) = ldftn($Invoke3);
			*(data + 662896) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new TroopSquadRequest((UIntPtr)0);
		}
	}
}
