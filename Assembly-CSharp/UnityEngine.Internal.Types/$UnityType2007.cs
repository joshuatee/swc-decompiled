using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2007 : $UnityType
	{
		public unsafe $UnityType2007()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 837252) = ldftn($Invoke0);
			*(data + 837280) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ActivateMissionStoryAction((UIntPtr)0);
		}
	}
}
