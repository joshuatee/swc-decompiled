using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2076 : $UnityType
	{
		public unsafe $UnityType2076()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 842348) = ldftn($Invoke0);
			*(data + 842376) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new StartPlaceBuildingStoryAction((UIntPtr)0);
		}
	}
}
