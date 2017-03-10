using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2024 : $UnityType
	{
		public unsafe $UnityType2024()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 838428) = ldftn($Invoke0);
			*(data + 838456) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DelayStoryAction((UIntPtr)0);
		}
	}
}
