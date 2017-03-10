using StaRTS.Main.Models.Commands.Pvp;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1541 : $UnityType
	{
		public unsafe $UnityType1541()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 658948) = ldftn($Invoke0);
			*(data + 658976) = ldftn($Invoke1);
			*(data + 659004) = ldftn($Invoke2);
			*(data + 659032) = ldftn($Invoke3);
			*(data + 659060) = ldftn($Invoke4);
			*(data + 659088) = ldftn($Invoke5);
			*(data + 659116) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new PvpBattleStartRequest((UIntPtr)0);
		}
	}
}
