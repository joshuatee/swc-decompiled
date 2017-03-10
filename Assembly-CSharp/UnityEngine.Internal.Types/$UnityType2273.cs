using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2273 : $UnityType
	{
		public unsafe $UnityType2273()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 906160) = ldftn($Invoke0);
			*(data + 906188) = ldftn($Invoke1);
			*(data + 906216) = ldftn($Invoke2);
			*(data + 906244) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new AbstractSquadWarInfoScreenTab((UIntPtr)0);
		}
	}
}
