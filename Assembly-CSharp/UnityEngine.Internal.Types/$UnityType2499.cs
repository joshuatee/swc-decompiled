using StaRTS.Main.Views.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2499 : $UnityType
	{
		public unsafe $UnityType2499()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 978736) = ldftn($Invoke0);
			*(data + 978764) = ldftn($Invoke1);
			*(data + 978792) = ldftn($Invoke2);
			*(data + 978820) = ldftn($Invoke3);
			*(data + 978848) = ldftn($Invoke4);
			*(data + 978876) = ldftn($Invoke5);
			*(data + 978904) = ldftn($Invoke6);
			*(data + 978932) = ldftn($Invoke7);
			*(data + 978960) = ldftn($Invoke8);
			*(data + 978988) = ldftn($Invoke9);
			*(data + 979016) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new DynamicRadiusView((UIntPtr)0);
		}
	}
}
