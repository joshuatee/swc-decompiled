using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2065 : $UnityType
	{
		public unsafe $UnityType2065()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 841284) = ldftn($Invoke0);
			*(data + 841312) = ldftn($Invoke1);
			*(data + 841340) = ldftn($Invoke2);
			*(data + 841368) = ldftn($Invoke3);
			*(data + 841396) = ldftn($Invoke4);
			*(data + 841424) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new ShowHologramStoryAction((UIntPtr)0);
		}
	}
}
