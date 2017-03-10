using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2053 : $UnityType
	{
		public unsafe $UnityType2053()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 840304) = ldftn($Invoke0);
			*(data + 840332) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new PressHereStoryAction((UIntPtr)0);
		}
	}
}
