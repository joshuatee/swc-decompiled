using StaRTS.Main.Controllers.GameStates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType825 : $UnityType
	{
		public unsafe $UnityType825()
		{
			*(UnityEngine.Internal.$Metadata.data + 573100) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new BattleEndPlaybackState((UIntPtr)0);
		}
	}
}
