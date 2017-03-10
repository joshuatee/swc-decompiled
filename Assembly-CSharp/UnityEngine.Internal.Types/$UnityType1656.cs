using StaRTS.Main.Models.Commands.Squads.Responses;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1656 : $UnityType
	{
		public unsafe $UnityType1656()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 664520) = ldftn($Invoke0);
			*(data + 664548) = ldftn($Invoke1);
			*(data + 664576) = ldftn($Invoke2);
			*(data + 664604) = ldftn($Invoke3);
			*(data + 664632) = ldftn($Invoke4);
			*(data + 664660) = ldftn($Invoke5);
			*(data + 664688) = ldftn($Invoke6);
			*(data + 664716) = ldftn($Invoke7);
			*(data + 664744) = ldftn($Invoke8);
			*(data + 664772) = ldftn($Invoke9);
			*(data + 664800) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new TroopDonateResponse((UIntPtr)0);
		}
	}
}
