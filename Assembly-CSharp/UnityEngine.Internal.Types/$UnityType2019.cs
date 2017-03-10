using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2019 : $UnityType
	{
		public unsafe $UnityType2019()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 838148) = ldftn($Invoke0);
			*(data + 838176) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ConfigureControlsStoryAction((UIntPtr)0);
		}
	}
}
