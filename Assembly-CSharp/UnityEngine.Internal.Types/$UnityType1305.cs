using StaRTS.Main.Models.Commands;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1305 : $UnityType
	{
		public unsafe $UnityType1305()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 649540) = ldftn($Invoke0);
			*(data + 649568) = ldftn($Invoke1);
			*(data + 649596) = ldftn($Invoke2);
			*(data + 649624) = ldftn($Invoke3);
			*(data + 649652) = ldftn($Invoke4);
			*(data + 649680) = ldftn($Invoke5);
			*(data + 649708) = ldftn($Invoke6);
			*(data + 649736) = ldftn($Invoke7);
			*(data + 649764) = ldftn($Invoke8);
			*(data + 649792) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new PlayerIdChecksumRequest((UIntPtr)0);
		}
	}
}
