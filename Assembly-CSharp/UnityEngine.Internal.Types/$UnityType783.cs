using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType783 : $UnityType
	{
		public unsafe $UnityType783()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 565848) = ldftn($Invoke0);
			*(data + 565876) = ldftn($Invoke1);
			*(data + 565904) = ldftn($Invoke2);
			*(data + 565932) = ldftn($Invoke3);
			*(data + 565960) = ldftn($Invoke4);
			*(data + 565988) = ldftn($Invoke5);
			*(data + 566016) = ldftn($Invoke6);
			*(data + 566044) = ldftn($Invoke7);
			*(data + 566072) = ldftn($Invoke8);
			*(data + 566100) = ldftn($Invoke9);
			*(data + 566128) = ldftn($Invoke10);
			*(data + 566156) = ldftn($Invoke11);
			*(data + 566184) = ldftn($Invoke12);
			*(data + 566212) = ldftn($Invoke13);
			*(data + 566240) = ldftn($Invoke14);
			*(data + 566268) = ldftn($Invoke15);
			*(data + 566296) = ldftn($Invoke16);
		}

		public override object CreateInstance()
		{
			return new UXController((UIntPtr)0);
		}
	}
}
