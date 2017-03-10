using StaRTS.Main.Views.UX.Tags;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2475 : $UnityType
	{
		public unsafe $UnityType2475()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 970952) = ldftn($Invoke0);
			*(data + 970980) = ldftn($Invoke1);
			*(data + 971008) = ldftn($Invoke2);
			*(data + 971036) = ldftn($Invoke3);
			*(data + 971064) = ldftn($Invoke4);
			*(data + 971092) = ldftn($Invoke5);
			*(data + 971120) = ldftn($Invoke6);
			*(data + 971148) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new PlayerVisitTag((UIntPtr)0);
		}
	}
}
