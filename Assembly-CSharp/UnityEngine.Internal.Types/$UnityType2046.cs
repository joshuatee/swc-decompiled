using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2046 : $UnityType
	{
		public unsafe $UnityType2046()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 839800) = ldftn($Invoke0);
			*(data + 839828) = ldftn($Invoke1);
			*(data + 839856) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new OpenStoreScreenStoryAction((UIntPtr)0);
		}
	}
}
