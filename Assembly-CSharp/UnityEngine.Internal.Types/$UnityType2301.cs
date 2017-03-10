using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2301 : $UnityType
	{
		public unsafe $UnityType2301()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 916632) = ldftn($Invoke0);
			*(data + 916660) = ldftn($Invoke1);
			*(data + 916688) = ldftn($Invoke2);
			*(data + 916716) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new FactionFlipConfirmationScreen((UIntPtr)0);
		}
	}
}
