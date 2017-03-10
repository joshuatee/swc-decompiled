using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2102 : $UnityType
	{
		public unsafe $UnityType2102()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 845316) = ldftn($Invoke0);
			*(data + 845344) = ldftn($Invoke1);
			*(data + 845372) = ldftn($Invoke2);
			*(data + 845400) = ldftn($Invoke3);
			*(data + 845428) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new SquadSizeStoryTrigger((UIntPtr)0);
		}
	}
}
