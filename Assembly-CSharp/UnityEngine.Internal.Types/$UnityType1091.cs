using StaRTS.Main.Models.Battle;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1091 : $UnityType
	{
		public unsafe $UnityType1091()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 635820) = ldftn($Invoke0);
			*(data + 635848) = ldftn($Invoke1);
			*(data + 635876) = ldftn($Invoke2);
			*(data + 635904) = ldftn($Invoke3);
			*(data + 635932) = ldftn($Invoke4);
			*(data + 635960) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new LootData((UIntPtr)0);
		}
	}
}
