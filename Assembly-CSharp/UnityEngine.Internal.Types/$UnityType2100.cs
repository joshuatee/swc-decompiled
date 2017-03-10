using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2100 : $UnityType
	{
		public unsafe $UnityType2100()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 845064) = ldftn($Invoke0);
			*(data + 845092) = ldftn($Invoke1);
			*(data + 845120) = ldftn($Invoke2);
			*(data + 845148) = ldftn($Invoke3);
			*(data + 845176) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new SquadActiveMembersStoryTrigger((UIntPtr)0);
		}
	}
}
