using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2008 : $UnityType
	{
		public unsafe $UnityType2008()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 837308) = ldftn($Invoke0);
			*(data + 837336) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ActivateTriggerStoryAction((UIntPtr)0);
		}
	}
}
