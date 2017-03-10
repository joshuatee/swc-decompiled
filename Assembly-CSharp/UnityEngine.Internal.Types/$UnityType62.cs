using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType62 : $UnityType
	{
		public unsafe $UnityType62()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 359320) = ldftn($Invoke0);
			*(data + 359348) = ldftn($Invoke1);
			*(data + 359376) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new PanToPlanetStoryAction((UIntPtr)0);
		}
	}
}
