using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2313 : $UnityType
	{
		public unsafe $UnityType2313()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 920356) = ldftn($Invoke0);
			*(data + 920384) = ldftn($Invoke1);
			*(data + 920412) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new LimitedEditionItemPurchaseConfirmationScreen((UIntPtr)0);
		}
	}
}
