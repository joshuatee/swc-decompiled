using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType104 : $UnityType
	{
		public unsafe $UnityType104()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 370688) = ldftn($Invoke0);
			*(data + 370716) = ldftn($Invoke1);
			*(data + 370744) = ldftn($Invoke2);
			*(data + 370772) = ldftn($Invoke3);
			*(data + 370800) = ldftn($Invoke4);
			*(data + 370828) = ldftn($Invoke5);
			*(data + 370856) = ldftn($Invoke6);
			*(data + 370884) = ldftn($Invoke7);
			*(data + 370912) = ldftn($Invoke8);
			*(data + 370940) = ldftn($Invoke9);
			*(data + 370968) = ldftn($Invoke10);
			*(data + 370996) = ldftn($Invoke11);
			*(data + 371024) = ldftn($Invoke12);
			*(data + 371052) = ldftn($Invoke13);
			*(data + 371080) = ldftn($Invoke14);
			*(data + 371108) = ldftn($Invoke15);
			*(data + 371136) = ldftn($Invoke16);
			*(data + 371164) = ldftn($Invoke17);
			*(data + 1524328) = ldftn($Get0);
			*(data + 1524332) = ldftn($Set0);
			*(data + 1524344) = ldftn($Get1);
			*(data + 1524348) = ldftn($Set1);
			*(data + 1524360) = ldftn($Get2);
			*(data + 1524364) = ldftn($Set2);
		}

		public override object CreateInstance()
		{
			return new TweenPosition((UIntPtr)0);
		}
	}
}
