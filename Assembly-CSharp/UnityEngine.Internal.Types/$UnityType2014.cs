using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2014 : $UnityType
	{
		public unsafe $UnityType2014()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 837812) = ldftn($Invoke0);
			*(data + 837840) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ClearButtonCircleStoryAction((UIntPtr)0);
		}
	}
}
