using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType112 : $UnityType
	{
		public unsafe $UnityType112()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 374020) = ldftn($Invoke0);
			*(data + 374048) = ldftn($Invoke1);
			*(data + 374076) = ldftn($Invoke2);
			*(data + 374104) = ldftn($Invoke3);
			*(data + 374132) = ldftn($Invoke4);
			*(data + 374160) = ldftn($Invoke5);
			*(data + 374188) = ldftn($Invoke6);
			*(data + 374216) = ldftn($Invoke7);
			*(data + 374244) = ldftn($Invoke8);
			*(data + 374272) = ldftn($Invoke9);
			*(data + 374300) = ldftn($Invoke10);
			*(data + 374328) = ldftn($Invoke11);
			*(data + 374356) = ldftn($Invoke12);
			*(data + 374384) = ldftn($Invoke13);
			*(data + 1524760) = ldftn($Get0);
			*(data + 1524764) = ldftn($Set0);
			*(data + 1524776) = ldftn($Get1);
			*(data + 1524780) = ldftn($Set1);
		}

		public override object CreateInstance()
		{
			return new UI2DSpriteAnimation((UIntPtr)0);
		}
	}
}
