using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2035 : $UnityType
	{
		public unsafe $UnityType2035()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 839072) = ldftn($Invoke0);
			*(data + 839100) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new EndChainStoryAction((UIntPtr)0);
		}
	}
}
