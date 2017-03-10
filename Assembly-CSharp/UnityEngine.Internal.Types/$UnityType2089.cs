using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2089 : $UnityType
	{
		public unsafe $UnityType2089()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 843720) = ldftn($Invoke0);
			*(data + 843748) = ldftn($Invoke1);
			*(data + 843776) = ldftn($Invoke2);
			*(data + 843804) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new BuildingRepairStoryTrigger((UIntPtr)0);
		}
	}
}
