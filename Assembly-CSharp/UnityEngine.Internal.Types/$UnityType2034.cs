using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2034 : $UnityType
	{
		public unsafe $UnityType2034()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 839016) = ldftn($Invoke0);
			*(data + 839044) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new EnableGridScrollingStoryAction((UIntPtr)0);
		}
	}
}
