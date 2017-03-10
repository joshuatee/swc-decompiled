using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2025 : $UnityType
	{
		public unsafe $UnityType2025()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 838484) = ldftn($Invoke0);
			*(data + 838512) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DeployBuildingStoryAction((UIntPtr)0);
		}
	}
}
