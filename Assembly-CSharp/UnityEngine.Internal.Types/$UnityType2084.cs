using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2084 : $UnityType
	{
		public unsafe $UnityType2084()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 843076) = ldftn($Invoke0);
			*(data + 843104) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ZoomCameraStoryAction((UIntPtr)0);
		}
	}
}
