using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2030 : $UnityType
	{
		public unsafe $UnityType2030()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 838764) = ldftn($Invoke0);
			*(data + 838792) = ldftn($Invoke1);
			*(data + 838820) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new DisplayButtonStoryAction((UIntPtr)0);
		}
	}
}
