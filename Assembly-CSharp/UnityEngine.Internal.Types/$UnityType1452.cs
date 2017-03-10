using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1452 : $UnityType
	{
		public unsafe $UnityType1452()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 654664) = ldftn($Invoke0);
			*(data + 654692) = ldftn($Invoke1);
			*(data + 654720) = ldftn($Invoke2);
			*(data + 654748) = ldftn($Invoke3);
			*(data + 654776) = ldftn($Invoke4);
			*(data + 654804) = ldftn($Invoke5);
			*(data + 654832) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new MoneyReceiptVerifyResponse((UIntPtr)0);
		}
	}
}
