using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2051 : $UnityType
	{
		public unsafe $UnityType2051()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 840136) = ldftn($Invoke0);
			*(data + 840164) = ldftn($Invoke1);
			*(data + 840192) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new PlayHoloAnimationStoryAction((UIntPtr)0);
		}
	}
}
