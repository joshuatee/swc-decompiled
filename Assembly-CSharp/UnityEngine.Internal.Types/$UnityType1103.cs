using StaRTS.Main.Models.Battle.Replay;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1103 : $UnityType
	{
		public unsafe $UnityType1103()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 640104) = ldftn($Invoke0);
			*(data + 640132) = ldftn($Invoke1);
			*(data + 640160) = ldftn($Invoke2);
			*(data + 640188) = ldftn($Invoke3);
			*(data + 640216) = ldftn($Invoke4);
			*(data + 640244) = ldftn($Invoke5);
			*(data + 640272) = ldftn($Invoke6);
			*(data + 640300) = ldftn($Invoke7);
			*(data + 640328) = ldftn($Invoke8);
			*(data + 640356) = ldftn($Invoke9);
			*(data + 640384) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new SpecialAttackDeployedAction((UIntPtr)0);
		}
	}
}
