using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2367 : $UnityType
	{
		public unsafe $UnityType2367()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 936204) = ldftn($Invoke0);
			*(data + 936232) = ldftn($Invoke1);
			*(data + 936260) = ldftn($Invoke2);
			*(data + 936288) = ldftn($Invoke3);
			*(data + 936316) = ldftn($Invoke4);
			*(data + 936344) = ldftn($Invoke5);
			*(data + 936372) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new TournamentEndedScreen((UIntPtr)0);
		}
	}
}
