using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType760 : $UnityType
	{
		public unsafe $UnityType760()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 551624) = ldftn($Invoke0);
			*(data + 551652) = ldftn($Invoke1);
			*(data + 551680) = ldftn($Invoke2);
			*(data + 551708) = ldftn($Invoke3);
			*(data + 551736) = ldftn($Invoke4);
			*(data + 551764) = ldftn($Invoke5);
			*(data + 551792) = ldftn($Invoke6);
			*(data + 551820) = ldftn($Invoke7);
			*(data + 551848) = ldftn($Invoke8);
			*(data + 551876) = ldftn($Invoke9);
			*(data + 551904) = ldftn($Invoke10);
			*(data + 551932) = ldftn($Invoke11);
			*(data + 551960) = ldftn($Invoke12);
			*(data + 551988) = ldftn($Invoke13);
			*(data + 552016) = ldftn($Invoke14);
			*(data + 552044) = ldftn($Invoke15);
			*(data + 552072) = ldftn($Invoke16);
		}

		public override object CreateInstance()
		{
			return new ShooterController((UIntPtr)0);
		}
	}
}
