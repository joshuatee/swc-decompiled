using StaRTS.Main.Story.Trigger;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2085 : $UnityType
	{
		public unsafe $UnityType2085()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 843132) = ldftn($Invoke0);
			*(data + 843160) = ldftn($Invoke1);
			*(data + 843188) = ldftn($Invoke2);
			*(data + 843216) = ldftn($Invoke3);
			*(data + 843244) = ldftn($Invoke4);
			*(data + 843272) = ldftn($Invoke5);
			*(data + 843300) = ldftn($Invoke6);
			*(data + 843328) = ldftn($Invoke7);
			*(data + 843356) = ldftn($Invoke8);
			*(data + 843384) = ldftn($Invoke9);
			*(data + 843412) = ldftn($Invoke10);
			*(data + 843440) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new AbstractStoryTrigger((UIntPtr)0);
		}
	}
}
