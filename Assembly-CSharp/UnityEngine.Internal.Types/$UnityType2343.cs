using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2343 : $UnityType
	{
		public unsafe $UnityType2343()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 930184) = ldftn($Invoke0);
			*(data + 930212) = ldftn($Invoke1);
			*(data + 930240) = ldftn($Invoke2);
			*(data + 930268) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new SquadIntroScreen((UIntPtr)0);
		}
	}
}
