using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2031 : $UnityType
	{
		public unsafe $UnityType2031()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 838848) = ldftn($Invoke0);
			*(data + 838876) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new EditPrefStoryAction((UIntPtr)0);
		}
	}
}
