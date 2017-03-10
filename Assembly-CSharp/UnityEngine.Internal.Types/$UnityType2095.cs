using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2095 : $UnityType
	{
		public unsafe $UnityType2095()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 844532) = ldftn($Invoke0);
			*(data + 844560) = ldftn($Invoke1);
			*(data + 844588) = ldftn($Invoke2);
			*(data + 844616) = ldftn($Invoke3);
			*(data + 844644) = ldftn($Invoke4);
			*(data + 844672) = ldftn($Invoke5);
			*(data + 844700) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new HQLevelStoryTrigger((UIntPtr)0);
		}
	}
}
