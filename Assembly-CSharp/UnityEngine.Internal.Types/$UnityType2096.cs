using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2096 : $UnityType
	{
		public unsafe $UnityType2096()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 844728) = ldftn($Invoke0);
			*(data + 844756) = ldftn($Invoke1);
			*(data + 844784) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new MissionVictoryStoryTrigger((UIntPtr)0);
		}
	}
}
