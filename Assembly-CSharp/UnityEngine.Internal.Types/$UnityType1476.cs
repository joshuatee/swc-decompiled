using StaRTS.Main.Models.Commands.Player.Account.External;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1476 : $UnityType
	{
		public unsafe $UnityType1476()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 655588) = ldftn($Invoke0);
			*(data + 655616) = ldftn($Invoke1);
			*(data + 655644) = ldftn($Invoke2);
			*(data + 655672) = ldftn($Invoke3);
			*(data + 655700) = ldftn($Invoke4);
			*(data + 655728) = ldftn($Invoke5);
			*(data + 655756) = ldftn($Invoke6);
			*(data + 655784) = ldftn($Invoke7);
			*(data + 655812) = ldftn($Invoke8);
			*(data + 655840) = ldftn($Invoke9);
			*(data + 655868) = ldftn($Invoke10);
			*(data + 655896) = ldftn($Invoke11);
			*(data + 655924) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new RegisterExternalAccountRequest((UIntPtr)0);
		}
	}
}
