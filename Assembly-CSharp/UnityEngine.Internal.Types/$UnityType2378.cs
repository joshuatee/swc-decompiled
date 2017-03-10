using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2378 : $UnityType
	{
		public unsafe $UnityType2378()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 941020) = ldftn($Invoke0);
			*(data + 941048) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new TwoButtonFueScreen((UIntPtr)0);
		}
	}
}
