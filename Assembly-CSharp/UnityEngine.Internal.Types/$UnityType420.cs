using StaRTS.Externals.IAP;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType420 : $UnityType
	{
		public unsafe $UnityType420()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 445504) = ldftn($Invoke0);
			*(data + 445532) = ldftn($Invoke1);
			*(data + 445560) = ldftn($Invoke2);
			*(data + 445588) = ldftn($Invoke3);
			*(data + 445616) = ldftn($Invoke4);
			*(data + 445644) = ldftn($Invoke5);
			*(data + 445672) = ldftn($Invoke6);
			*(data + 445700) = ldftn($Invoke7);
			*(data + 445728) = ldftn($Invoke8);
			*(data + 445756) = ldftn($Invoke9);
			*(data + 445784) = ldftn($Invoke10);
			*(data + 445812) = ldftn($Invoke11);
			*(data + 445840) = ldftn($Invoke12);
			*(data + 445868) = ldftn($Invoke13);
			*(data + 445896) = ldftn($Invoke14);
			*(data + 445924) = ldftn($Invoke15);
			*(data + 445952) = ldftn($Invoke16);
			*(data + 445980) = ldftn($Invoke17);
			*(data + 446008) = ldftn($Invoke18);
			*(data + 446036) = ldftn($Invoke19);
			*(data + 446064) = ldftn($Invoke20);
			*(data + 446092) = ldftn($Invoke21);
			*(data + 446120) = ldftn($Invoke22);
			*(data + 446148) = ldftn($Invoke23);
		}

		public override object CreateInstance()
		{
			return new InAppPurchaseController((UIntPtr)0);
		}
	}
}
