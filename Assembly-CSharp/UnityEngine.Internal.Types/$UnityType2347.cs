using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2347 : $UnityType
	{
		public unsafe $UnityType2347()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 931024) = ldftn($Invoke0);
			*(data + 931052) = ldftn($Invoke1);
			*(data + 931080) = ldftn($Invoke2);
			*(data + 931108) = ldftn($Invoke3);
			*(data + 931136) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new SquadWarEndCelebrationScreen((UIntPtr)0);
		}
	}
}
