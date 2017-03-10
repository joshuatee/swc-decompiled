using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2338 : $UnityType
	{
		public unsafe $UnityType2338()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 928784) = ldftn($Invoke0);
			*(data + 928812) = ldftn($Invoke1);
			*(data + 928840) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new ShieldGeneratorInfoScreen((UIntPtr)0);
		}
	}
}
