using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2058 : $UnityType
	{
		public unsafe $UnityType2058()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 840640) = ldftn($Invoke0);
			*(data + 840668) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new SaveProgressStoryAction((UIntPtr)0);
		}
	}
}
