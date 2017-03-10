using StaRTS.Main.Story;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2005 : $UnityType
	{
		public unsafe $UnityType2005()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 837000) = ldftn($Invoke0);
			*(data + 837028) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new StoryTriggerFactory((UIntPtr)0);
		}
	}
}
