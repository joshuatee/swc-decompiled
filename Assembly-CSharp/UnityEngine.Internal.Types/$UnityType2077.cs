using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2077 : $UnityType
	{
		public unsafe $UnityType2077()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 842404) = ldftn($Invoke0);
			*(data + 842432) = ldftn($Invoke1);
			*(data + 842460) = ldftn($Invoke2);
			*(data + 842488) = ldftn($Invoke3);
			*(data + 842516) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new StoreLookupStoryAction((UIntPtr)0);
		}
	}
}
