using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType40 : $UnityType
	{
		public unsafe $UnityType40()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 352936) = ldftn($Invoke0);
			*(data + 352964) = ldftn($Invoke1);
			*(data + 352992) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new GalaxyMapOpenStoryTrigger((UIntPtr)0);
		}
	}
}
