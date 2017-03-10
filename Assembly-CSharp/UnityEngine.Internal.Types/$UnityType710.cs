using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType710 : $UnityType
	{
		public unsafe $UnityType710()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 535132) = ldftn($Invoke0);
			*(data + 535160) = ldftn($Invoke1);
			*(data + 535188) = ldftn($Invoke2);
			*(data + 535216) = ldftn($Invoke3);
			*(data + 535244) = ldftn($Invoke4);
			*(data + 535272) = ldftn($Invoke5);
			*(data + 535300) = ldftn($Invoke6);
			*(data + 535328) = ldftn($Invoke7);
			*(data + 535356) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new EntityRenderController((UIntPtr)0);
		}
	}
}
