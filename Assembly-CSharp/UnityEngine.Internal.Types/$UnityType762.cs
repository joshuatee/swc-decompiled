using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType762 : $UnityType
	{
		public unsafe $UnityType762()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 552548) = ldftn($Invoke0);
			*(data + 552576) = ldftn($Invoke1);
			*(data + 552604) = ldftn($Invoke2);
			*(data + 552632) = ldftn($Invoke3);
			*(data + 552660) = ldftn($Invoke4);
			*(data + 552688) = ldftn($Invoke5);
			*(data + 552716) = ldftn($Invoke6);
			*(data + 552744) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new SkinController((UIntPtr)0);
		}
	}
}
