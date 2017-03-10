using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1329 : $UnityType
	{
		public unsafe $UnityType1329()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 650268) = ldftn($Invoke0);
			*(data + 650296) = ldftn($Invoke1);
			*(data + 650324) = ldftn($Invoke2);
			*(data + 650352) = ldftn($Invoke3);
			*(data + 650380) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new CheatGetBattleRecordResponse((UIntPtr)0);
		}
	}
}
