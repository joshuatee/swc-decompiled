using StaRTS.Main.Models.Commands.Squads.Responses;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1652 : $UnityType
	{
		public unsafe $UnityType1652()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 663960) = ldftn($Invoke0);
			*(data + 663988) = ldftn($Invoke1);
			*(data + 664016) = ldftn($Invoke2);
			*(data + 664044) = ldftn($Invoke3);
			*(data + 664072) = ldftn($Invoke4);
			*(data + 664100) = ldftn($Invoke5);
			*(data + 664128) = ldftn($Invoke6);
			*(data + 664156) = ldftn($Invoke7);
			*(data + 664184) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new SquadMemberWarDataResponse((UIntPtr)0);
		}
	}
}
