using StaRTS.Main.Views.UserInput;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2488 : $UnityType
	{
		public unsafe $UnityType2488()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 975376) = ldftn($Invoke0);
			*(data + 975404) = ldftn($Invoke1);
			*(data + 975432) = ldftn($Invoke2);
			*(data + 975460) = ldftn($Invoke3);
			*(data + 975488) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new InputManager((UIntPtr)0);
		}
	}
}
