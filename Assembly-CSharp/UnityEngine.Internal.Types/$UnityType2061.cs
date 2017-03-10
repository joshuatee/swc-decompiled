using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2061 : $UnityType
	{
		public unsafe $UnityType2061()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 840808) = ldftn($Invoke0);
			*(data + 840836) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ShowBuildingTooltipByTypeStoryAction((UIntPtr)0);
		}
	}
}
