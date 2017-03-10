using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2069 : $UnityType
	{
		public unsafe $UnityType2069()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 841760) = ldftn($Invoke0);
			*(data + 841788) = ldftn($Invoke1);
			*(data + 841816) = ldftn($Invoke2);
			*(data + 841844) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new ShowScreenStoryAction((UIntPtr)0);
		}
	}
}
