using StaRTS.Main.Controllers.GameStates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType826 : $UnityType
	{
		public unsafe $UnityType826()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 573128) = ldftn($Invoke0);
			*(data + 573156) = ldftn($Invoke1);
			*(data + 573184) = ldftn($Invoke2);
			*(data + 573212) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new BattleEndState((UIntPtr)0);
		}
	}
}
