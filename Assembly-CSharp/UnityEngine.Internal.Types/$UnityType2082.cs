using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2082 : $UnityType
	{
		public unsafe $UnityType2082()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 842964) = ldftn($Invoke0);
			*(data + 842992) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new UndimScreenStoryAction((UIntPtr)0);
		}
	}
}
