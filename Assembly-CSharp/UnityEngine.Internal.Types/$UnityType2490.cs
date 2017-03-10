using StaRTS.Main.Views.UserInput;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2490 : $UnityType
	{
		public unsafe $UnityType2490()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 975600) = ldftn($Invoke0);
			*(data + 975628) = ldftn($Invoke1);
			*(data + 975656) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new TouchManager((UIntPtr)0);
		}
	}
}
