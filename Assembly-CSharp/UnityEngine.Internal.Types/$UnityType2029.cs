using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2029 : $UnityType
	{
		public unsafe $UnityType2029()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 838708) = ldftn($Invoke0);
			*(data + 838736) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DisableGridScrollingStoryAction((UIntPtr)0);
		}
	}
}
