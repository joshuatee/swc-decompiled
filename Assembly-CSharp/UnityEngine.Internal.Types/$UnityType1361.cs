using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1361 : $UnityType
	{
		public unsafe $UnityType1361()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 651192) = ldftn($Invoke0);
			*(data + 651220) = ldftn($Invoke1);
			*(data + 651248) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new CheatSetTroopDonateRepRequest((UIntPtr)0);
		}
	}
}
