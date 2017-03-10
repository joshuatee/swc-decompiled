using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2022 : $UnityType
	{
		public unsafe $UnityType2022()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 838316) = ldftn($Invoke0);
			*(data + 838344) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DegenerateStoryAction((UIntPtr)0);
		}
	}
}
