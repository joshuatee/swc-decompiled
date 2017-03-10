using StaRTS.Main.Story;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2003 : $UnityType
	{
		public unsafe $UnityType2003()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 836888) = ldftn($Invoke0);
			*(data + 836916) = ldftn($Invoke1);
			*(data + 836944) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new SessionStartTriggerParent((UIntPtr)0);
		}
	}
}
