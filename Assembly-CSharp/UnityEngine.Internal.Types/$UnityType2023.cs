using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2023 : $UnityType
	{
		public unsafe $UnityType2023()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 838372) = ldftn($Invoke0);
			*(data + 838400) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DelayBlockingStoryAction((UIntPtr)0);
		}
	}
}
