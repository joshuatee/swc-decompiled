using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2066 : $UnityType
	{
		public unsafe $UnityType2066()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 841452) = ldftn($Invoke0);
			*(data + 841480) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ShowInstructionStoryAction((UIntPtr)0);
		}
	}
}
