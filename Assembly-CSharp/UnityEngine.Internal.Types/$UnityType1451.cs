using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1451 : $UnityType
	{
		public unsafe $UnityType1451()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 654468) = ldftn($Invoke0);
			*(data + 654496) = ldftn($Invoke1);
			*(data + 654524) = ldftn($Invoke2);
			*(data + 654552) = ldftn($Invoke3);
			*(data + 654580) = ldftn($Invoke4);
			*(data + 654608) = ldftn($Invoke5);
			*(data + 654636) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new MoneyReceiptVerifyRequest((UIntPtr)0);
		}
	}
}
