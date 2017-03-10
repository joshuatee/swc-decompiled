using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2028 : $UnityType
	{
		public unsafe $UnityType2028()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 838652) = ldftn($Invoke0);
			*(data + 838680) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DisableClicksStoryAction((UIntPtr)0);
		}
	}
}
