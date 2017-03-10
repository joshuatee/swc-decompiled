using StaRTS.Main.Models.Commands.Pvp;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1539 : $UnityType
	{
		public unsafe $UnityType1539()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 658808) = ldftn($Invoke0);
			*(data + 658836) = ldftn($Invoke1);
			*(data + 658864) = ldftn($Invoke2);
			*(data + 658892) = ldftn($Invoke3);
			*(data + 658920) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new PvpBattleEndResponse((UIntPtr)0);
		}
	}
}
