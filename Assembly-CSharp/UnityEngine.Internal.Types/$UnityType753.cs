using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType753 : $UnityType
	{
		public unsafe $UnityType753()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 548628) = ldftn($Invoke0);
			*(data + 548656) = ldftn($Invoke1);
			*(data + 548684) = ldftn($Invoke2);
			*(data + 548712) = ldftn($Invoke3);
			*(data + 548740) = ldftn($Invoke4);
			*(data + 548768) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new RewardTag((UIntPtr)0);
		}
	}
}
