using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType89 : $UnityType
	{
		public unsafe $UnityType89()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 364276) = ldftn($Invoke0);
			*(data + 364304) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new TapjoyEventRequest((UIntPtr)0);
		}
	}
}
