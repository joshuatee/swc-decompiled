using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2020 : $UnityType
	{
		public unsafe $UnityType2020()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 838204) = ldftn($Invoke0);
			*(data + 838232) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DeactivateTriggerStoryAction((UIntPtr)0);
		}
	}
}
