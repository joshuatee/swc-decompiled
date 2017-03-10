using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2086 : $UnityType
	{
		public unsafe $UnityType2086()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 843468) = ldftn($Invoke0);
			*(data + 843496) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new AutoStoryTrigger((UIntPtr)0);
		}
	}
}
