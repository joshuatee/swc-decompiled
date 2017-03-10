using StaRTS.Main.Models.Player.Misc;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1830 : $UnityType
	{
		public unsafe $UnityType1830()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 700220) = ldftn($Invoke0);
			*(data + 700248) = ldftn($Invoke1);
			*(data + 700276) = ldftn($Invoke2);
			*(data + 700304) = ldftn($Invoke3);
			*(data + 700332) = ldftn($Invoke4);
			*(data + 700360) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new UnlockedLevelData((UIntPtr)0);
		}
	}
}
