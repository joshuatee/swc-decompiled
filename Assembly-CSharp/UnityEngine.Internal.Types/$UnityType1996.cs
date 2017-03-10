using StaRTS.Main.Story;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1996 : $UnityType
	{
		public unsafe $UnityType1996()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 836384) = ldftn($Invoke0);
			*(data + 836412) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DebugStoryReactor((UIntPtr)0);
		}
	}
}
