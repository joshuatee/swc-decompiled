using StaRTS.Main.Views.UserInput;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2484 : $UnityType
	{
		public unsafe $UnityType2484()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 975124) = ldftn($Invoke0);
			*(data + 975152) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DefaultBackButtonManager((UIntPtr)0);
		}
	}
}
