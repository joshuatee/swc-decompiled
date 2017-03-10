using StaRTS.Main.Views.UX.Elements;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2271 : $UnityType
	{
		public unsafe $UnityType2271()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 905152) = ldftn($Invoke0);
			*(data + 905180) = ldftn($Invoke1);
			*(data + 905208) = ldftn($Invoke2);
			*(data + 905236) = ldftn($Invoke3);
			*(data + 905264) = ldftn($Invoke4);
			*(data + 905292) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new UXTween((UIntPtr)0);
		}
	}
}
