using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType105 : $UnityType
	{
		public unsafe $UnityType105()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 371192) = ldftn($Invoke0);
			*(data + 371220) = ldftn($Invoke1);
			*(data + 371248) = ldftn($Invoke2);
			*(data + 371276) = ldftn($Invoke3);
			*(data + 371304) = ldftn($Invoke4);
			*(data + 371332) = ldftn($Invoke5);
			*(data + 371360) = ldftn($Invoke6);
			*(data + 371388) = ldftn($Invoke7);
			*(data + 371416) = ldftn($Invoke8);
			*(data + 371444) = ldftn($Invoke9);
			*(data + 371472) = ldftn($Invoke10);
			*(data + 371500) = ldftn($Invoke11);
			*(data + 371528) = ldftn($Invoke12);
			*(data + 371556) = ldftn($Invoke13);
			*(data + 371584) = ldftn($Invoke14);
			*(data + 371612) = ldftn($Invoke15);
			*(data + 1524376) = ldftn($Get0);
			*(data + 1524380) = ldftn($Set0);
			*(data + 1524392) = ldftn($Get1);
			*(data + 1524396) = ldftn($Set1);
			*(data + 1524408) = ldftn($Get2);
			*(data + 1524412) = ldftn($Set2);
		}

		public override object CreateInstance()
		{
			return new TweenRotation((UIntPtr)0);
		}
	}
}
