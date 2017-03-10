using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2357 : $UnityType
	{
		public unsafe $UnityType2357()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 933376) = ldftn($Invoke0);
			*(data + 933404) = ldftn($Invoke1);
			*(data + 933432) = ldftn($Invoke2);
			*(data + 933460) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new SquadWarStartScreen((UIntPtr)0);
		}
	}
}
