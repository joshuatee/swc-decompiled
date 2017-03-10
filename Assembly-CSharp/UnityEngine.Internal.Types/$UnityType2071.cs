using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2071 : $UnityType
	{
		public unsafe $UnityType2071()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 841956) = ldftn($Invoke0);
			*(data + 841984) = ldftn($Invoke1);
			*(data + 842012) = ldftn($Invoke2);
			*(data + 842040) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new ShowTranscriptStoryAction((UIntPtr)0);
		}
	}
}
