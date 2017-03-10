using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2311 : $UnityType
	{
		public unsafe $UnityType2311()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 919544) = ldftn($Invoke0);
			*(data + 919572) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new IAPDisclaimerScreen((UIntPtr)0);
		}
	}
}
