using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2328 : $UnityType
	{
		public unsafe $UnityType2328()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 924976) = ldftn($Invoke0);
			*(data + 925004) = ldftn($Invoke1);
			*(data + 925032) = ldftn($Invoke2);
			*(data + 925060) = ldftn($Invoke3);
			*(data + 925088) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new ProcessingScreen((UIntPtr)0);
		}
	}
}
