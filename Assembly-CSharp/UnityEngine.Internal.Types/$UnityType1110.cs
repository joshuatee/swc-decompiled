using StaRTS.Main.Models.Commands;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1110 : $UnityType
	{
		public unsafe $UnityType1110()
		{
			*(UnityEngine.Internal.$Metadata.data + 641056) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new BattleEndRequest((UIntPtr)0);
		}
	}
}
