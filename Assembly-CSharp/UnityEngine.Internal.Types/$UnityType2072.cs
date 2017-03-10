using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2072 : $UnityType
	{
		public unsafe $UnityType2072()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 842068) = ldftn($Invoke0);
			*(data + 842096) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ShowUIElementStoryAction((UIntPtr)0);
		}
	}
}
