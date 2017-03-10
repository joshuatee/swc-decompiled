using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2094 : $UnityType
	{
		public unsafe $UnityType2094()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 844420) = ldftn($Invoke0);
			*(data + 844448) = ldftn($Invoke1);
			*(data + 844476) = ldftn($Invoke2);
			*(data + 844504) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new GameStateStoryTrigger((UIntPtr)0);
		}
	}
}
