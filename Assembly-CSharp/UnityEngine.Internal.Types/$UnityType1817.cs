using StaRTS.Main.Models.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1817 : $UnityType
	{
		public unsafe $UnityType1817()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 693360) = ldftn($Invoke0);
			*(data + 693388) = ldftn($Invoke1);
			*(data + 693416) = ldftn($Invoke2);
			*(data + 693444) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new SharedPlayerPrefs((UIntPtr)0);
		}
	}
}
