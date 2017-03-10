using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2274 : $UnityType
	{
		public unsafe $UnityType2274()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 906272) = ldftn($Invoke0);
			*(data + 906300) = ldftn($Invoke1);
			*(data + 906328) = ldftn($Invoke2);
			*(data + 906356) = ldftn($Invoke3);
			*(data + 906384) = ldftn($Invoke4);
			*(data + 906412) = ldftn($Invoke5);
			*(data + 906440) = ldftn($Invoke6);
			*(data + 906468) = ldftn($Invoke7);
			*(data + 906496) = ldftn($Invoke8);
			*(data + 906524) = ldftn($Invoke9);
			*(data + 906552) = ldftn($Invoke10);
			*(data + 906580) = ldftn($Invoke11);
			*(data + 906608) = ldftn($Invoke12);
			*(data + 906636) = ldftn($Invoke13);
			*(data + 906664) = ldftn($Invoke14);
			*(data + 906692) = ldftn($Invoke15);
		}

		public override object CreateInstance()
		{
			return new AccountSyncScreen((UIntPtr)0);
		}
	}
}
