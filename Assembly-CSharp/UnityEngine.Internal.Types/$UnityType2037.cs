using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2037 : $UnityType
	{
		public unsafe $UnityType2037()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 839184) = ldftn($Invoke0);
			*(data + 839212) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ExitEditModeStoryAction((UIntPtr)0);
		}
	}
}
