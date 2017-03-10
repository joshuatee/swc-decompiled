using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2050 : $UnityType
	{
		public unsafe $UnityType2050()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 840080) = ldftn($Invoke0);
			*(data + 840108) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new PlayAudioStoryAction((UIntPtr)0);
		}
	}
}
