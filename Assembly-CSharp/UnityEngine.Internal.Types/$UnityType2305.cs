using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2305 : $UnityType
	{
		public unsafe $UnityType2305()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 917584) = ldftn($Invoke0);
			*(data + 917612) = ldftn($Invoke1);
			*(data + 917640) = ldftn($Invoke2);
			*(data + 917668) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new GeneratorInfoScreen((UIntPtr)0);
		}
	}
}
