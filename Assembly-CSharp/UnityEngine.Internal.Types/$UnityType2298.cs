using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2298 : $UnityType
	{
		public unsafe $UnityType2298()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 915260) = ldftn($Invoke0);
			*(data + 915288) = ldftn($Invoke1);
			*(data + 915316) = ldftn($Invoke2);
			*(data + 915344) = ldftn($Invoke3);
			*(data + 915372) = ldftn($Invoke4);
			*(data + 915400) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new DisableProtectionAlertScreen((UIntPtr)0);
		}
	}
}
