using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2080 : $UnityType
	{
		public unsafe $UnityType2080()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 842796) = ldftn($Invoke0);
			*(data + 842824) = ldftn($Invoke1);
			*(data + 842852) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new TransitionToHomeStoryAction((UIntPtr)0);
		}
	}
}
