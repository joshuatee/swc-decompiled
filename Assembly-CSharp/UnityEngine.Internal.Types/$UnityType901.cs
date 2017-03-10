using StaRTS.Main.Controllers.ServerMessages;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType901 : $UnityType
	{
		public unsafe $UnityType901()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 589676) = ldftn($Invoke0);
			*(data + 589704) = ldftn($Invoke1);
			*(data + 589732) = ldftn($Invoke2);
			*(data + 589760) = ldftn($Invoke3);
			*(data + 589788) = ldftn($Invoke4);
			*(data + 589816) = ldftn($Invoke5);
			*(data + 589844) = ldftn($Invoke6);
			*(data + 589872) = ldftn($Invoke7);
			*(data + 589900) = ldftn($Invoke8);
			*(data + 589928) = ldftn($Invoke9);
			*(data + 589956) = ldftn($Invoke10);
			*(data + 589984) = ldftn($Invoke11);
			*(data + 590012) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new IAPReceiptServerMessage((UIntPtr)0);
		}
	}
}
