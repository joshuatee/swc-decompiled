using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2045 : $UnityType
	{
		public unsafe $UnityType2045()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 839744) = ldftn($Invoke0);
			*(data + 839772) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new MoveCameraStoryAction((UIntPtr)0);
		}
	}
}
