using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2087 : $UnityType
	{
		public unsafe $UnityType2087()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 843524) = ldftn($Invoke0);
			*(data + 843552) = ldftn($Invoke1);
			*(data + 843580) = ldftn($Invoke2);
			*(data + 843608) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new BattleCompleteStoryTrigger((UIntPtr)0);
		}
	}
}
