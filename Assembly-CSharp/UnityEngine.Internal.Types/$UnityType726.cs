using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType726 : $UnityType
	{
		public unsafe $UnityType726()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 541124) = ldftn($Invoke0);
			*(data + 541152) = ldftn($Invoke1);
			*(data + 541180) = ldftn($Invoke2);
			*(data + 541208) = ldftn($Invoke3);
			*(data + 541236) = ldftn($Invoke4);
			*(data + 541264) = ldftn($Invoke5);
			*(data + 541292) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new LightingEffectsController((UIntPtr)0);
		}
	}
}
