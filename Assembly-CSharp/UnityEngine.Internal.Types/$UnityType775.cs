using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType775 : $UnityType
	{
		public unsafe $UnityType775()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 564224) = ldftn($Invoke0);
			*(data + 564252) = ldftn($Invoke1);
			*(data + 564280) = ldftn($Invoke2);
			*(data + 564308) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new TrapViewController((UIntPtr)0);
		}
	}
}
