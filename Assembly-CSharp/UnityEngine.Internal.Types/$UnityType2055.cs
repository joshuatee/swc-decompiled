using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2055 : $UnityType
	{
		public unsafe $UnityType2055()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 840416) = ldftn($Invoke0);
			*(data + 840444) = ldftn($Invoke1);
			*(data + 840472) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new RebelEmpireForkingStoryAction((UIntPtr)0);
		}
	}
}
