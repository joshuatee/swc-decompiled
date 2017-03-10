using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2013 : $UnityType
	{
		public unsafe $UnityType2013()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 837756) = ldftn($Invoke0);
			*(data + 837784) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ClearBuildingHighlightStoryAction((UIntPtr)0);
		}
	}
}
