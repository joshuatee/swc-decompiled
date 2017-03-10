using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2040 : $UnityType
	{
		public unsafe $UnityType2040()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 839380) = ldftn($Invoke0);
			*(data + 839408) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new HideInstructionStoryAction((UIntPtr)0);
		}
	}
}
