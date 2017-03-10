using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2083 : $UnityType
	{
		public unsafe $UnityType2083()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 843020) = ldftn($Invoke0);
			*(data + 843048) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new UnpauseBuildingRepairStoryAction((UIntPtr)0);
		}
	}
}
