using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1450 : $UnityType
	{
		public unsafe $UnityType1450()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 654272) = ldftn($Invoke0);
			*(data + 654300) = ldftn($Invoke1);
			*(data + 654328) = ldftn($Invoke2);
			*(data + 654356) = ldftn($Invoke3);
			*(data + 654384) = ldftn($Invoke4);
			*(data + 654412) = ldftn($Invoke5);
			*(data + 654440) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new MoneyReceiptVerifyCommand((UIntPtr)0);
		}
	}
}
