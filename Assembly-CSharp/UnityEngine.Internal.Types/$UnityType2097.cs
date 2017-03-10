using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2097 : $UnityType
	{
		public unsafe $UnityType2097()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 844812) = ldftn($Invoke0);
			*(data + 844840) = ldftn($Invoke1);
			*(data + 844868) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new PlanetRelocateStoryTrigger((UIntPtr)0);
		}
	}
}
