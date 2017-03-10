using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2021 : $UnityType
	{
		public unsafe $UnityType2021()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 838260) = ldftn($Invoke0);
			*(data + 838288) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DefendBaseStoryAction((UIntPtr)0);
		}
	}
}
