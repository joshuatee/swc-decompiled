using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2057 : $UnityType
	{
		public unsafe $UnityType2057()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 840584) = ldftn($Invoke0);
			*(data + 840612) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ResumeBattleStoryAction((UIntPtr)0);
		}
	}
}
