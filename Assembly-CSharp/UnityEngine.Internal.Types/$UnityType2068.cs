using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2068 : $UnityType
	{
		public unsafe $UnityType2068()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 841648) = ldftn($Invoke0);
			*(data + 841676) = ldftn($Invoke1);
			*(data + 841704) = ldftn($Invoke2);
			*(data + 841732) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new ShowRateAppScreenStoryAction((UIntPtr)0);
		}
	}
}
