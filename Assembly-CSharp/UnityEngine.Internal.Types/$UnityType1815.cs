using StaRTS.Main.Models.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1815 : $UnityType
	{
		public unsafe $UnityType1815()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 693192) = ldftn($Invoke0);
			*(data + 693220) = ldftn($Invoke1);
			*(data + 693248) = ldftn($Invoke2);
			*(data + 693276) = ldftn($Invoke3);
			*(data + 693304) = ldftn($Invoke4);
			*(data + 693332) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new ServerPlayerPrefs((UIntPtr)0);
		}
	}
}
