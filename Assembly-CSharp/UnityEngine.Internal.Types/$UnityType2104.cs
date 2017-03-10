using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2104 : $UnityType
	{
		public unsafe $UnityType2104()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 845596) = ldftn($Invoke0);
			*(data + 845624) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new TimerTrigger((UIntPtr)0);
		}
	}
}
