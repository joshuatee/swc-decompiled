using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2308 : $UnityType
	{
		public unsafe $UnityType2308()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 918088) = ldftn($Invoke0);
			*(data + 918116) = ldftn($Invoke1);
			*(data + 918144) = ldftn($Invoke2);
			*(data + 918172) = ldftn($Invoke3);
			*(data + 918200) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new HQUpgradeScreen((UIntPtr)0);
		}
	}
}
