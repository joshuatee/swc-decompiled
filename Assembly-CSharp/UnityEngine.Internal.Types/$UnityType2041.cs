using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2041 : $UnityType
	{
		public unsafe $UnityType2041()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 839436) = ldftn($Invoke0);
			*(data + 839464) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new HideTranscriptStoryAction((UIntPtr)0);
		}
	}
}
