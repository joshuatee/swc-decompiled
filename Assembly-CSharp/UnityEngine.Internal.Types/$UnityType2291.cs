using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2291 : $UnityType
	{
		public unsafe $UnityType2291()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 912348) = ldftn($Invoke0);
			*(data + 912376) = ldftn($Invoke1);
			*(data + 912404) = ldftn($Invoke2);
			*(data + 912432) = ldftn($Invoke3);
			*(data + 912460) = ldftn($Invoke4);
			*(data + 912488) = ldftn($Invoke5);
			*(data + 912516) = ldftn($Invoke6);
			*(data + 912544) = ldftn($Invoke7);
			*(data + 912572) = ldftn($Invoke8);
			*(data + 912600) = ldftn($Invoke9);
			*(data + 912628) = ldftn($Invoke10);
			*(data + 912656) = ldftn($Invoke11);
			*(data + 912684) = ldftn($Invoke12);
			*(data + 912712) = ldftn($Invoke13);
		}

		public override object CreateInstance()
		{
			return new ConfirmRelocateScreen((UIntPtr)0);
		}
	}
}
