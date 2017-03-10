using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2049 : $UnityType
	{
		public unsafe $UnityType2049()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 840024) = ldftn($Invoke0);
			*(data + 840052) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new PauseBuildingRepairStoryAction((UIntPtr)0);
		}
	}
}
