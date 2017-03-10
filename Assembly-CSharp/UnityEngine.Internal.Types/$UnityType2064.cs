using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2064 : $UnityType
	{
		public unsafe $UnityType2064()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 841004) = ldftn($Invoke0);
			*(data + 841032) = ldftn($Invoke1);
			*(data + 841060) = ldftn($Invoke2);
			*(data + 841088) = ldftn($Invoke3);
			*(data + 841116) = ldftn($Invoke4);
			*(data + 841144) = ldftn($Invoke5);
			*(data + 841172) = ldftn($Invoke6);
			*(data + 841200) = ldftn($Invoke7);
			*(data + 841228) = ldftn($Invoke8);
			*(data + 841256) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new ShowHologramInfoStoryAction((UIntPtr)0);
		}
	}
}
