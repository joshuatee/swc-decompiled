using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2092 : $UnityType
	{
		public unsafe $UnityType2092()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 844028) = ldftn($Invoke0);
			*(data + 844056) = ldftn($Invoke1);
			*(data + 844084) = ldftn($Invoke2);
			*(data + 844112) = ldftn($Invoke3);
			*(data + 844140) = ldftn($Invoke4);
			*(data + 844168) = ldftn($Invoke5);
			*(data + 844196) = ldftn($Invoke6);
			*(data + 844224) = ldftn($Invoke7);
			*(data + 844252) = ldftn($Invoke8);
			*(data + 844280) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new EventCounterStoryTrigger((UIntPtr)0);
		}
	}
}
