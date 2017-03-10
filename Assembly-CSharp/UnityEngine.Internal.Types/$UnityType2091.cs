using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2091 : $UnityType
	{
		public unsafe $UnityType2091()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 843916) = ldftn($Invoke0);
			*(data + 843944) = ldftn($Invoke1);
			*(data + 843972) = ldftn($Invoke2);
			*(data + 844000) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new ClusterORDERStoryTrigger((UIntPtr)0);
		}
	}
}
