using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2105 : $UnityType
	{
		public unsafe $UnityType2105()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 845652) = ldftn($Invoke0);
			*(data + 845680) = ldftn($Invoke1);
			*(data + 845708) = ldftn($Invoke2);
			*(data + 845736) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new UnlockPlanetTrigger((UIntPtr)0);
		}
	}
}
