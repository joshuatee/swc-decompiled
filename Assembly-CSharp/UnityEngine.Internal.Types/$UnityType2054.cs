using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2054 : $UnityType
	{
		public unsafe $UnityType2054()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 840360) = ldftn($Invoke0);
			*(data + 840388) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new PromptPvpStoryAction((UIntPtr)0);
		}
	}
}
