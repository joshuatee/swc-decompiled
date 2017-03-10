using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2047 : $UnityType
	{
		public unsafe $UnityType2047()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 839884) = ldftn($Invoke0);
			*(data + 839912) = ldftn($Invoke1);
			*(data + 839940) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new OpenWarInfoStoryAction((UIntPtr)0);
		}
	}
}
