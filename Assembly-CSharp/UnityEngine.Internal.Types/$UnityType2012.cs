using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2012 : $UnityType
	{
		public unsafe $UnityType2012()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 837700) = ldftn($Invoke0);
			*(data + 837728) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new CircleRegionStoryAction((UIntPtr)0);
		}
	}
}
