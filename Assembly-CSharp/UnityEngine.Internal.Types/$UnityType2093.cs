using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2093 : $UnityType
	{
		public unsafe $UnityType2093()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 844308) = ldftn($Invoke0);
			*(data + 844336) = ldftn($Invoke1);
			*(data + 844364) = ldftn($Invoke2);
			*(data + 844392) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new FactionChangedStoryTrigger((UIntPtr)0);
		}
	}
}
