using StaRTS.Main.Views.UserInput;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2489 : $UnityType
	{
		public unsafe $UnityType2489()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 975516) = ldftn($Invoke0);
			*(data + 975544) = ldftn($Invoke1);
			*(data + 975572) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new MouseManager((UIntPtr)0);
		}
	}
}
