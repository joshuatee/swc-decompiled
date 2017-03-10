using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType108 : $UnityType
	{
		public unsafe $UnityType108()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 372312) = ldftn($Invoke0);
			*(data + 372340) = ldftn($Invoke1);
			*(data + 372368) = ldftn($Invoke2);
			*(data + 372396) = ldftn($Invoke3);
			*(data + 372424) = ldftn($Invoke4);
			*(data + 372452) = ldftn($Invoke5);
			*(data + 372480) = ldftn($Invoke6);
			*(data + 372508) = ldftn($Invoke7);
			*(data + 372536) = ldftn($Invoke8);
			*(data + 372564) = ldftn($Invoke9);
			*(data + 372592) = ldftn($Invoke10);
			*(data + 372620) = ldftn($Invoke11);
			*(data + 372648) = ldftn($Invoke12);
			*(data + 372676) = ldftn($Invoke13);
			*(data + 1524520) = ldftn($Get0);
			*(data + 1524524) = ldftn($Set0);
			*(data + 1524536) = ldftn($Get1);
			*(data + 1524540) = ldftn($Set1);
		}

		public override object CreateInstance()
		{
			return new TweenVolume((UIntPtr)0);
		}
	}
}
