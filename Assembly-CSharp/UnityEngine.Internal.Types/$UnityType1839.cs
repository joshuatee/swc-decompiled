using StaRTS.Main.Models.Player.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1839 : $UnityType
	{
		public unsafe $UnityType1839()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 703048) = ldftn($Invoke0);
			*(data + 703076) = ldftn($Invoke1);
			*(data + 703104) = ldftn($Invoke2);
			*(data + 703132) = ldftn($Invoke3);
			*(data + 703160) = ldftn($Invoke4);
			*(data + 703188) = ldftn($Invoke5);
			*(data + 703216) = ldftn($Invoke6);
			*(data + 703244) = ldftn($Invoke7);
			*(data + 703272) = ldftn($Invoke8);
			*(data + 703300) = ldftn($Invoke9);
			*(data + 703328) = ldftn($Invoke10);
			*(data + 703356) = ldftn($Invoke11);
			*(data + 703384) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new ContractTO((UIntPtr)0);
		}
	}
}
