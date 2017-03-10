using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2056 : $UnityType
	{
		public unsafe $UnityType2056()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 840500) = ldftn($Invoke0);
			*(data + 840528) = ldftn($Invoke1);
			*(data + 840556) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new RemoveBuildingStoryAction((UIntPtr)0);
		}
	}
}
