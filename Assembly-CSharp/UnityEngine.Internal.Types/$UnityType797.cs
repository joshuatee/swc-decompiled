using StaRTS.Main.Controllers.CombineMesh;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType797 : $UnityType
	{
		public unsafe $UnityType797()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 569264) = ldftn($Invoke0);
			*(data + 569292) = ldftn($Invoke1);
			*(data + 569320) = ldftn($Invoke2);
			*(data + 569348) = ldftn($Invoke3);
			*(data + 569376) = ldftn($Invoke4);
			*(data + 569404) = ldftn($Invoke5);
			*(data + 569432) = ldftn($Invoke6);
			*(data + 569460) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new HomeBaseCombineMeshHelper((UIntPtr)0);
		}
	}
}
