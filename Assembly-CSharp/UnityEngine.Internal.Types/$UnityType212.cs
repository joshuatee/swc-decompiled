using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType212 : $UnityType
	{
		public unsafe $UnityType212()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 408880) = ldftn($Invoke0);
			*(data + 408908) = ldftn($Invoke1);
			*(data + 408936) = ldftn($Invoke2);
			*(data + 408964) = ldftn($Invoke3);
			*(data + 408992) = ldftn($Invoke4);
			*(data + 409020) = ldftn($Invoke5);
			*(data + 409048) = ldftn($Invoke6);
			*(data + 409076) = ldftn($Invoke7);
			*(data + 409104) = ldftn($Invoke8);
			*(data + 409132) = ldftn($Invoke9);
			*(data + 409160) = ldftn($Invoke10);
			*(data + 409188) = ldftn($Invoke11);
			*(data + 409216) = ldftn($Invoke12);
			*(data + 409244) = ldftn($Invoke13);
			*(data + 1528424) = ldftn($Get0);
			*(data + 1528428) = ldftn($Set0);
			*(data + 1528440) = ldftn($Get1);
			*(data + 1528444) = ldftn($Set1);
			*(data + 1528456) = ldftn($Get2);
			*(data + 1528460) = ldftn($Set2);
		}

		public override object CreateInstance()
		{
			return new UITable((UIntPtr)0);
		}
	}
}
