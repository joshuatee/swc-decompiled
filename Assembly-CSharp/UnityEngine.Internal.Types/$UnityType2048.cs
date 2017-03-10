using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2048 : $UnityType
	{
		public unsafe $UnityType2048()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 839968) = ldftn($Invoke0);
			*(data + 839996) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new PauseBattleStoryAction((UIntPtr)0);
		}
	}
}
