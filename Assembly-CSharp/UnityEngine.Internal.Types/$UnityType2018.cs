using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2018 : $UnityType
	{
		public unsafe $UnityType2018()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 838036) = ldftn($Invoke0);
			*(data + 838064) = ldftn($Invoke1);
			*(data + 838092) = ldftn($Invoke2);
			*(data + 838120) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new ClusterANDStoryAction((UIntPtr)0);
		}
	}
}
