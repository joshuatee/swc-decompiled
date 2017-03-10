using StaRTS.Main.Views.UX.Elements;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2265 : $UnityType
	{
		public unsafe $UnityType2265()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 902828) = ldftn($Invoke0);
			*(data + 902856) = ldftn($Invoke1);
			*(data + 902884) = ldftn($Invoke2);
			*(data + 902912) = ldftn($Invoke3);
			*(data + 902940) = ldftn($Invoke4);
			*(data + 902968) = ldftn($Invoke5);
			*(data + 902996) = ldftn($Invoke6);
			*(data + 903024) = ldftn($Invoke7);
			*(data + 903052) = ldftn($Invoke8);
			*(data + 903080) = ldftn($Invoke9);
			*(data + 903108) = ldftn($Invoke10);
			*(data + 903136) = ldftn($Invoke11);
			*(data + 903164) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new UXSprite((UIntPtr)0);
		}
	}
}
