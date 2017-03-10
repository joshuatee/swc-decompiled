using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2044 : $UnityType
	{
		public unsafe $UnityType2044()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 839660) = ldftn($Invoke0);
			*(data + 839688) = ldftn($Invoke1);
			*(data + 839716) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new MainFUEGateStoryAction((UIntPtr)0);
		}
	}
}
