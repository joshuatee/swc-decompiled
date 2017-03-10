using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1454 : $UnityType
	{
		public unsafe $UnityType1454()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 654860) = ldftn($Invoke0);
			*(data + 654888) = ldftn($Invoke1);
			*(data + 654916) = ldftn($Invoke2);
			*(data + 654944) = ldftn($Invoke3);
			*(data + 654972) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new PlayerErrorRequest((UIntPtr)0);
		}
	}
}
