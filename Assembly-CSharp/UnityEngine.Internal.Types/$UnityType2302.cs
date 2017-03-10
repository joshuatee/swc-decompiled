using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2302 : $UnityType
	{
		public unsafe $UnityType2302()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 916744) = ldftn($Invoke0);
			*(data + 916772) = ldftn($Invoke1);
			*(data + 916800) = ldftn($Invoke2);
			*(data + 916828) = ldftn($Invoke3);
			*(data + 916856) = ldftn($Invoke4);
			*(data + 916884) = ldftn($Invoke5);
			*(data + 916912) = ldftn($Invoke6);
			*(data + 916940) = ldftn($Invoke7);
			*(data + 916968) = ldftn($Invoke8);
			*(data + 916996) = ldftn($Invoke9);
			*(data + 917024) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new FactionFlipScreen((UIntPtr)0);
		}
	}
}
