using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2059 : $UnityType
	{
		public unsafe $UnityType2059()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 840696) = ldftn($Invoke0);
			*(data + 840724) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new SetBuildingRepairStateStoryAction((UIntPtr)0);
		}
	}
}
