using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2099 : $UnityType
	{
		public unsafe $UnityType2099()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 844952) = ldftn($Invoke0);
			*(data + 844980) = ldftn($Invoke1);
			*(data + 845008) = ldftn($Invoke2);
			*(data + 845036) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new ScreenAppearStoryTrigger((UIntPtr)0);
		}
	}
}
