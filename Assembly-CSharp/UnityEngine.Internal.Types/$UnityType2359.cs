using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2359 : $UnityType
	{
		public unsafe $UnityType2359()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 933656) = ldftn($Invoke0);
			*(data + 933684) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new StarportUpgradeScreen((UIntPtr)0);
		}
	}
}
