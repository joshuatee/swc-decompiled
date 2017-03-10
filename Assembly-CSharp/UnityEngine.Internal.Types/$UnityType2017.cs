using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2017 : $UnityType
	{
		public unsafe $UnityType2017()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 837980) = ldftn($Invoke0);
			*(data + 838008) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new CloseScreenStoryAction((UIntPtr)0);
		}
	}
}
