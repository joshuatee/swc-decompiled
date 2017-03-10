using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2063 : $UnityType
	{
		public unsafe $UnityType2063()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 840920) = ldftn($Invoke0);
			*(data + 840948) = ldftn($Invoke1);
			*(data + 840976) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new ShowChooseFactionScreenStoryAction((UIntPtr)0);
		}
	}
}
