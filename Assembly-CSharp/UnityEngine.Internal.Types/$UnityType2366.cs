using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2366 : $UnityType
	{
		public unsafe $UnityType2366()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 936092) = ldftn($Invoke0);
			*(data + 936120) = ldftn($Invoke1);
			*(data + 936148) = ldftn($Invoke2);
			*(data + 936176) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new TimerAlertScreen((UIntPtr)0);
		}
	}
}
