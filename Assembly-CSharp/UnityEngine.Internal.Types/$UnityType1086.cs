using StaRTS.Main.Models.Battle;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1086 : $UnityType
	{
		public unsafe $UnityType1086()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 635036) = ldftn($Invoke0);
			*(data + 635064) = ldftn($Invoke1);
			*(data + 635092) = ldftn($Invoke2);
			*(data + 635120) = ldftn($Invoke3);
			*(data + 635148) = ldftn($Invoke4);
			*(data + 635176) = ldftn($Invoke5);
			*(data + 635204) = ldftn($Invoke6);
			*(data + 635232) = ldftn($Invoke7);
			*(data + 635260) = ldftn($Invoke8);
			*(data + 635288) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new DefenseTroopGroup((UIntPtr)0);
		}
	}
}
