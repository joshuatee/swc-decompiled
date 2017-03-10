using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType182 : $UnityType
	{
		public unsafe $UnityType182()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 398996) = ldftn($Invoke0);
			*(data + 399024) = ldftn($Invoke1);
			*(data + 399052) = ldftn($Invoke2);
			*(data + 399080) = ldftn($Invoke3);
			*(data + 399108) = ldftn($Invoke4);
			*(data + 399136) = ldftn($Invoke5);
			*(data + 399164) = ldftn($Invoke6);
			*(data + 399192) = ldftn($Invoke7);
			*(data + 399220) = ldftn($Invoke8);
			*(data + 399248) = ldftn($Invoke9);
			*(data + 399276) = ldftn($Invoke10);
			*(data + 399304) = ldftn($Invoke11);
			*(data + 399332) = ldftn($Invoke12);
			*(data + 1527240) = ldftn($Get0);
			*(data + 1527244) = ldftn($Set0);
			*(data + 1527256) = ldftn($Get1);
			*(data + 1527260) = ldftn($Set1);
			*(data + 1527272) = ldftn($Get2);
			*(data + 1527276) = ldftn($Set2);
		}

		public override object CreateInstance()
		{
			return new UIPlaySound((UIntPtr)0);
		}
	}
}
