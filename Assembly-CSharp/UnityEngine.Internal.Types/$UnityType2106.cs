using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2106 : $UnityType
	{
		public unsafe $UnityType2106()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 845764) = ldftn($Invoke0);
			*(data + 845792) = ldftn($Invoke1);
			*(data + 845820) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new WorldLoadStoryTrigger((UIntPtr)0);
		}
	}
}
