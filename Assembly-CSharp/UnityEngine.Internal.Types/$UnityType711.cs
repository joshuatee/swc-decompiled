using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType711 : $UnityType
	{
		public unsafe $UnityType711()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 535384) = ldftn($Invoke0);
			*(data + 535412) = ldftn($Invoke1);
			*(data + 535440) = ldftn($Invoke2);
			*(data + 535468) = ldftn($Invoke3);
			*(data + 535496) = ldftn($Invoke4);
			*(data + 535524) = ldftn($Invoke5);
			*(data + 535552) = ldftn($Invoke6);
			*(data + 535580) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new GameIdleController((UIntPtr)0);
		}
	}
}
