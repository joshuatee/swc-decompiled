using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2088 : $UnityType
	{
		public unsafe $UnityType2088()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 843636) = ldftn($Invoke0);
			*(data + 843664) = ldftn($Invoke1);
			*(data + 843692) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new BlockingWarStoryTrigger((UIntPtr)0);
		}
	}
}
