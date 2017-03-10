using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2075 : $UnityType
	{
		public unsafe $UnityType2075()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 842264) = ldftn($Invoke0);
			*(data + 842292) = ldftn($Invoke1);
			*(data + 842320) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new SpinPlanetForwardStoryAction((UIntPtr)0);
		}
	}
}
