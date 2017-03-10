using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType743 : $UnityType
	{
		public unsafe $UnityType743()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 545632) = ldftn($Invoke0);
			*(data + 545660) = ldftn($Invoke1);
			*(data + 545688) = ldftn($Invoke2);
			*(data + 545716) = ldftn($Invoke3);
			*(data + 545744) = ldftn($Invoke4);
			*(data + 545772) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new ProjectileController((UIntPtr)0);
		}
	}
}
