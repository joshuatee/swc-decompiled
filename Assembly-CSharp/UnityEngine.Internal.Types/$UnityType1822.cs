using StaRTS.Main.Models.Player.Misc;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1822 : $UnityType
	{
		public unsafe $UnityType1822()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 696020) = ldftn($Invoke0);
			*(data + 696048) = ldftn($Invoke1);
			*(data + 696076) = ldftn($Invoke2);
			*(data + 696104) = ldftn($Invoke3);
			*(data + 696132) = ldftn($Invoke4);
			*(data + 696160) = ldftn($Invoke5);
			*(data + 696188) = ldftn($Invoke6);
			*(data + 696216) = ldftn($Invoke7);
			*(data + 696244) = ldftn($Invoke8);
			*(data + 696272) = ldftn($Invoke9);
			*(data + 696300) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new BattleHistory((UIntPtr)0);
		}
	}
}
