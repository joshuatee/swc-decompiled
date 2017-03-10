using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2101 : $UnityType
	{
		public unsafe $UnityType2101()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 845204) = ldftn($Invoke0);
			*(data + 845232) = ldftn($Invoke1);
			*(data + 845260) = ldftn($Invoke2);
			*(data + 845288) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new SquadMemberStoryTrigger((UIntPtr)0);
		}
	}
}
