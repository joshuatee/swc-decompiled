using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2079 : $UnityType
	{
		public unsafe $UnityType2079()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 842656) = ldftn($Invoke0);
			*(data + 842684) = ldftn($Invoke1);
			*(data + 842712) = ldftn($Invoke2);
			*(data + 842740) = ldftn($Invoke3);
			*(data + 842768) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new TrainingInstructionsStoryAction((UIntPtr)0);
		}
	}
}
