using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2107 : $UnityType
	{
		public unsafe $UnityType2107()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 845848) = ldftn($Invoke0);
			*(data + 845876) = ldftn($Invoke1);
			*(data + 845904) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new WorldTransitionCompleteStoryTrigger((UIntPtr)0);
		}
	}
}
