using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2081 : $UnityType
	{
		public unsafe $UnityType2081()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 842880) = ldftn($Invoke0);
			*(data + 842908) = ldftn($Invoke1);
			*(data + 842936) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new TransitionToWorldStoryAction((UIntPtr)0);
		}
	}
}
