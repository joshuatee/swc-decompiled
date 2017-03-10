using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType738 : $UnityType
	{
		public unsafe $UnityType738()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 544512) = ldftn($Invoke0);
			*(data + 544540) = ldftn($Invoke1);
			*(data + 544568) = ldftn($Invoke2);
			*(data + 544596) = ldftn($Invoke3);
			*(data + 544624) = ldftn($Invoke4);
			*(data + 544652) = ldftn($Invoke5);
			*(data + 544680) = ldftn($Invoke6);
			*(data + 544708) = ldftn($Invoke7);
			*(data + 544736) = ldftn($Invoke8);
			*(data + 544764) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new PlayerIdentityController((UIntPtr)0);
		}
	}
}
