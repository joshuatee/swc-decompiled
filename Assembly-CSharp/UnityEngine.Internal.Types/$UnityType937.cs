using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType937 : $UnityType
	{
		public unsafe $UnityType937()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 597096) = ldftn($Invoke0);
			*(data + 597124) = ldftn($Invoke1);
			*(data + 597152) = ldftn($Invoke2);
			*(data + 597180) = ldftn($Invoke3);
			*(data + 597208) = ldftn($Invoke4);
			*(data + 597236) = ldftn($Invoke5);
			*(data + 597264) = ldftn($Invoke6);
			*(data + 597292) = ldftn($Invoke7);
			*(data + 597320) = ldftn($Invoke8);
			*(data + 597348) = ldftn($Invoke9);
			*(data + 597376) = ldftn($Invoke10);
			*(data + 597404) = ldftn($Invoke11);
			*(data + 597432) = ldftn($Invoke12);
			*(data + 597460) = ldftn($Invoke13);
			*(data + 597488) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new ServerStartupTask((UIntPtr)0);
		}
	}
}
