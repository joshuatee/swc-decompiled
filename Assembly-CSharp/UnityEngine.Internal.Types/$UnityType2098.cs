using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2098 : $UnityType
	{
		public unsafe $UnityType2098()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 844896) = ldftn($Invoke0);
			*(data + 844924) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ScoutPlanetTrigger((UIntPtr)0);
		}
	}
}
