using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2016 : $UnityType
	{
		public unsafe $UnityType2016()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 837924) = ldftn($Invoke0);
			*(data + 837952) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ClearProgressStoryAction((UIntPtr)0);
		}
	}
}
