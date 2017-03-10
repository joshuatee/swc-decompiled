using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2033 : $UnityType
	{
		public unsafe $UnityType2033()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 838960) = ldftn($Invoke0);
			*(data + 838988) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new EnableClicksStoryAction((UIntPtr)0);
		}
	}
}
