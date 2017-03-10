using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2074 : $UnityType
	{
		public unsafe $UnityType2074()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 842208) = ldftn($Invoke0);
			*(data + 842236) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new SpawnDefensiveTroopStoryAction((UIntPtr)0);
		}
	}
}
