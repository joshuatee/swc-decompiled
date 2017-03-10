using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType103 : $UnityType
	{
		public unsafe $UnityType103()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 370296) = ldftn($Invoke0);
			*(data + 370324) = ldftn($Invoke1);
			*(data + 370352) = ldftn($Invoke2);
			*(data + 370380) = ldftn($Invoke3);
			*(data + 370408) = ldftn($Invoke4);
			*(data + 370436) = ldftn($Invoke5);
			*(data + 370464) = ldftn($Invoke6);
			*(data + 370492) = ldftn($Invoke7);
			*(data + 370520) = ldftn($Invoke8);
			*(data + 370548) = ldftn($Invoke9);
			*(data + 370576) = ldftn($Invoke10);
			*(data + 370604) = ldftn($Invoke11);
			*(data + 370632) = ldftn($Invoke12);
			*(data + 370660) = ldftn($Invoke13);
			*(data + 1524296) = ldftn($Get0);
			*(data + 1524300) = ldftn($Set0);
			*(data + 1524312) = ldftn($Get1);
			*(data + 1524316) = ldftn($Set1);
		}

		public override object CreateInstance()
		{
			return new TweenOrthoSize((UIntPtr)0);
		}
	}
}
