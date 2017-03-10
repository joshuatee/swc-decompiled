using StaRTS.Main.Utils;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2110 : $UnityType
	{
		public unsafe $UnityType2110()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 846800) = ldftn($Invoke0);
			*(data + 846828) = ldftn($Invoke1);
			*(data + 846856) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new AutoStoryTriggerUtils((UIntPtr)0);
		}
	}
}
