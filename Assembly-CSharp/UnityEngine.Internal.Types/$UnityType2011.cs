using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2011 : $UnityType
	{
		public unsafe $UnityType2011()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 837476) = ldftn($Invoke0);
			*(data + 837504) = ldftn($Invoke1);
			*(data + 837532) = ldftn($Invoke2);
			*(data + 837560) = ldftn($Invoke3);
			*(data + 837588) = ldftn($Invoke4);
			*(data + 837616) = ldftn($Invoke5);
			*(data + 837644) = ldftn($Invoke6);
			*(data + 837672) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new CircleButtonStoryAction((UIntPtr)0);
		}
	}
}
