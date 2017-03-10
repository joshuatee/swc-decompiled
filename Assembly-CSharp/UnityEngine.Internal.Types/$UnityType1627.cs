using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1627 : $UnityType
	{
		public unsafe $UnityType1627()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 661132) = ldftn($Invoke0);
			*(data + 661160) = ldftn($Invoke1);
			*(data + 661188) = ldftn($Invoke2);
			*(data + 661216) = ldftn($Invoke3);
			*(data + 661244) = ldftn($Invoke4);
			*(data + 661272) = ldftn($Invoke5);
			*(data + 661300) = ldftn($Invoke6);
			*(data + 661328) = ldftn($Invoke7);
			*(data + 661356) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new EditSquadRequest((UIntPtr)0);
		}
	}
}
