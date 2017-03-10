using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType785 : $UnityType
	{
		public unsafe $UnityType785()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 566912) = ldftn($Invoke0);
			*(data + 566940) = ldftn($Invoke1);
			*(data + 566968) = ldftn($Invoke2);
			*(data + 566996) = ldftn($Invoke3);
			*(data + 567024) = ldftn($Invoke4);
			*(data + 567052) = ldftn($Invoke5);
			*(data + 567080) = ldftn($Invoke6);
			*(data + 567108) = ldftn($Invoke7);
			*(data + 567136) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new UserPlayerPrefsController((UIntPtr)0);
		}
	}
}
