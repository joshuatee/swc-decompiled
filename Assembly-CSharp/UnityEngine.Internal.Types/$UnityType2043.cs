using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2043 : $UnityType
	{
		public unsafe $UnityType2043()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 839576) = ldftn($Invoke0);
			*(data + 839604) = ldftn($Invoke1);
			*(data + 839632) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new IfPrefGateStoryAction((UIntPtr)0);
		}
	}
}
