using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType678 : $UnityType
	{
		public unsafe $UnityType678()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 518052) = ldftn($Invoke0);
			*(data + 518080) = ldftn($Invoke1);
			*(data + 518108) = ldftn($Invoke2);
			*(data + 518136) = ldftn($Invoke3);
			*(data + 518164) = ldftn($Invoke4);
			*(data + 518192) = ldftn($Invoke5);
			*(data + 518220) = ldftn($Invoke6);
			*(data + 518248) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new AchievementController((UIntPtr)0);
		}
	}
}
