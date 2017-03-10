using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2103 : $UnityType
	{
		public unsafe $UnityType2103()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 845456) = ldftn($Invoke0);
			*(data + 845484) = ldftn($Invoke1);
			*(data + 845512) = ldftn($Invoke2);
			*(data + 845540) = ldftn($Invoke3);
			*(data + 845568) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new SquadUIOpenStoryTrigger((UIntPtr)0);
		}
	}
}
