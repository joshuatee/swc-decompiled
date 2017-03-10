using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2036 : $UnityType
	{
		public unsafe $UnityType2036()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 839128) = ldftn($Invoke0);
			*(data + 839156) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new EndFueStoryAction((UIntPtr)0);
		}
	}
}
