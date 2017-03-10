using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2052 : $UnityType
	{
		public unsafe $UnityType2052()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 840220) = ldftn($Invoke0);
			*(data + 840248) = ldftn($Invoke1);
			*(data + 840276) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new PlayPlanetIntroStoryAction((UIntPtr)0);
		}
	}
}
