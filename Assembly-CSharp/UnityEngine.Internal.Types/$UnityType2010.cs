using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2010 : $UnityType
	{
		public unsafe $UnityType2010()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 837420) = ldftn($Invoke0);
			*(data + 837448) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new CircleBuildingStoryAction((UIntPtr)0);
		}
	}
}
