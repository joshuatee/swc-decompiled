using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1042 : $UnityType
	{
		public unsafe $UnityType1042()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 625376) = ldftn($Invoke0);
			*(data + 625404) = ldftn($Invoke1);
			*(data + 625432) = ldftn($Invoke2);
			*(data + 625460) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new HoloCommand((UIntPtr)0);
		}
	}
}
