using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2284 : $UnityType
	{
		public unsafe $UnityType2284()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 910108) = ldftn($Invoke0);
			*(data + 910136) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new BattleReplayShareScreen((UIntPtr)0);
		}
	}
}
