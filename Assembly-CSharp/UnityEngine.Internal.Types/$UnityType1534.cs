using StaRTS.Main.Models.Commands.Player.Store;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1534 : $UnityType
	{
		public unsafe $UnityType1534()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 658668) = ldftn($Invoke0);
			*(data + 658696) = ldftn($Invoke1);
			*(data + 658724) = ldftn($Invoke2);
			*(data + 658752) = ldftn($Invoke3);
			*(data + 658780) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new BuyResourceRequest((UIntPtr)0);
		}
	}
}
