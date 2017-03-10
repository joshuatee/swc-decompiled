using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2039 : $UnityType
	{
		public unsafe $UnityType2039()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 839296) = ldftn($Invoke0);
			*(data + 839324) = ldftn($Invoke1);
			*(data + 839352) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new HideHologramStoryAction((UIntPtr)0);
		}
	}
}
