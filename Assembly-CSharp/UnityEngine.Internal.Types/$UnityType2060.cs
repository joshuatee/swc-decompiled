using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2060 : $UnityType
	{
		public unsafe $UnityType2060()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 840752) = ldftn($Invoke0);
			*(data + 840780) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new SetMusicVolumeStoryAction((UIntPtr)0);
		}
	}
}
