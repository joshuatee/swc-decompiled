using StaRTS.Main.Controllers.GameStates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType827 : $UnityType
	{
		public unsafe $UnityType827()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 573240) = ldftn($Invoke0);
			*(data + 573268) = ldftn($Invoke1);
			*(data + 573296) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new BattlePlayState((UIntPtr)0);
		}
	}
}
