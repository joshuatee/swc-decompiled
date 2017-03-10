using StaRTS.Main.Models.Commands;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1306 : $UnityType
	{
		public unsafe $UnityType1306()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 649820) = ldftn($Invoke0);
			*(data + 649848) = ldftn($Invoke1);
			*(data + 649876) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new PlayerResourceResponse((UIntPtr)0);
		}
	}
}
