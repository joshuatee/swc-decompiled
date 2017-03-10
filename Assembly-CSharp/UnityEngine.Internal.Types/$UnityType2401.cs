using StaRTS.Main.Views.UX.Screens.ScreenHelpers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2401 : $UnityType
	{
		public unsafe $UnityType2401()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 947376) = ldftn($Invoke0);
			*(data + 947404) = ldftn($Invoke1);
			*(data + 947432) = ldftn($Invoke2);
			*(data + 947460) = ldftn($Invoke3);
			*(data + 947488) = ldftn($Invoke4);
			*(data + 947516) = ldftn($Invoke5);
			*(data + 947544) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new FactionDecal((UIntPtr)0);
		}
	}
}
