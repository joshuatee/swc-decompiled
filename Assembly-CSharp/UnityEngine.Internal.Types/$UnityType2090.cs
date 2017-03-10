using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2090 : $UnityType
	{
		public unsafe $UnityType2090()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 843832) = ldftn($Invoke0);
			*(data + 843860) = ldftn($Invoke1);
			*(data + 843888) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new ClusterANDStoryTrigger((UIntPtr)0);
		}
	}
}
