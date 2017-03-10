using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2038 : $UnityType
	{
		public unsafe $UnityType2038()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 839240) = ldftn($Invoke0);
			*(data + 839268) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new HideHoloInfoPanelStoryAction((UIntPtr)0);
		}
	}
}
