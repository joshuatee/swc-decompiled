using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2062 : $UnityType
	{
		public unsafe $UnityType2062()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 840864) = ldftn($Invoke0);
			*(data + 840892) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ShowBuildingTooltipsStoryAction((UIntPtr)0);
		}
	}
}
