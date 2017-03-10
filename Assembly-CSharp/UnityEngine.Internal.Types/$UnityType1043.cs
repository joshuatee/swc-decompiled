using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1043 : $UnityType
	{
		public unsafe $UnityType1043()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 625488) = ldftn($Invoke0);
			*(data + 625516) = ldftn($Invoke1);
			*(data + 625544) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new HudConfig((UIntPtr)0);
		}
	}
}
