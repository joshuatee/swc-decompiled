using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1935 : $UnityType
	{
		public unsafe $UnityType1935()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 755828) = ldftn($Invoke0);
			*(data + 755856) = ldftn($Invoke1);
			*(data + 755884) = ldftn($Invoke2);
			*(data + 755912) = ldftn($Invoke3);
			*(data + 755940) = ldftn($Invoke4);
			*(data + 755968) = ldftn($Invoke5);
			*(data + 755996) = ldftn($Invoke6);
			*(data + 756024) = ldftn($Invoke7);
			*(data + 756052) = ldftn($Invoke8);
			*(data + 756080) = ldftn($Invoke9);
			*(data + 756108) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new IconUpgradeVO((UIntPtr)0);
		}
	}
}
