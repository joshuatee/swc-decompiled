using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2042 : $UnityType
	{
		public unsafe $UnityType2042()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 839492) = ldftn($Invoke0);
			*(data + 839520) = ldftn($Invoke1);
			*(data + 839548) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new HighlightAreaStoryAction((UIntPtr)0);
		}
	}
}
