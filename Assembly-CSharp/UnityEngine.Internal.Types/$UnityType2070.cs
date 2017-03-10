using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2070 : $UnityType
	{
		public unsafe $UnityType2070()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 841872) = ldftn($Invoke0);
			*(data + 841900) = ldftn($Invoke1);
			*(data + 841928) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new ShowSetCallSignScreenStoryAction((UIntPtr)0);
		}
	}
}
