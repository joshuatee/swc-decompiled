using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2073 : $UnityType
	{
		public unsafe $UnityType2073()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 842124) = ldftn($Invoke0);
			*(data + 842152) = ldftn($Invoke1);
			*(data + 842180) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new ShowWhatsNextScreenStoryAction((UIntPtr)0);
		}
	}
}
