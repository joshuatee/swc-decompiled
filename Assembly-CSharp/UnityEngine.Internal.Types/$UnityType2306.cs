using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2306 : $UnityType
	{
		public unsafe $UnityType2306()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 917696) = ldftn($Invoke0);
			*(data + 917724) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new GeneratorUpgradeScreen((UIntPtr)0);
		}
	}
}
