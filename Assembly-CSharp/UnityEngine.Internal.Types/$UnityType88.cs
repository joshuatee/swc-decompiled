using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType88 : $UnityType
	{
		public unsafe $UnityType88()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 363940) = ldftn($Invoke0);
			*(data + 363968) = ldftn($Invoke1);
			*(data + 363996) = ldftn($Invoke2);
			*(data + 364024) = ldftn($Invoke3);
			*(data + 364052) = ldftn($Invoke4);
			*(data + 364080) = ldftn($Invoke5);
			*(data + 364108) = ldftn($Invoke6);
			*(data + 364136) = ldftn($Invoke7);
			*(data + 364164) = ldftn($Invoke8);
			*(data + 364192) = ldftn($Invoke9);
			*(data + 364220) = ldftn($Invoke10);
			*(data + 364248) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new TapjoyEvent((UIntPtr)0);
		}
	}
}
