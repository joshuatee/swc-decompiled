using StaRTS.Externals.IAP;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType421 : $UnityType
	{
		public unsafe $UnityType421()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 446176) = ldftn($Invoke0);
			*(data + 446204) = ldftn($Invoke1);
			*(data + 446232) = ldftn($Invoke2);
			*(data + 446260) = ldftn($Invoke3);
			*(data + 446288) = ldftn($Invoke4);
			*(data + 446316) = ldftn($Invoke5);
			*(data + 446344) = ldftn($Invoke6);
			*(data + 446372) = ldftn($Invoke7);
			*(data + 446400) = ldftn($Invoke8);
			*(data + 446428) = ldftn($Invoke9);
			*(data + 446456) = ldftn($Invoke10);
			*(data + 446484) = ldftn($Invoke11);
			*(data + 446512) = ldftn($Invoke12);
			*(data + 446540) = ldftn($Invoke13);
			*(data + 446568) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new InAppPurchaseProductInfo((UIntPtr)0);
		}
	}
}
