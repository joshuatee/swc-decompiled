using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType704 : $UnityType
	{
		public unsafe $UnityType704()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 533172) = ldftn($Invoke0);
			*(data + 533200) = ldftn($Invoke1);
			*(data + 533228) = ldftn($Invoke2);
			*(data + 533256) = ldftn($Invoke3);
			*(data + 533284) = ldftn($Invoke4);
			*(data + 533312) = ldftn($Invoke5);
			*(data + 533340) = ldftn($Invoke6);
			*(data + 533368) = ldftn($Invoke7);
			*(data + 533396) = ldftn($Invoke8);
			*(data + 533424) = ldftn($Invoke9);
			*(data + 533452) = ldftn($Invoke10);
			*(data + 533480) = ldftn($Invoke11);
			*(data + 533508) = ldftn($Invoke12);
			*(data + 533536) = ldftn($Invoke13);
			*(data + 533564) = ldftn($Invoke14);
			*(data + 533592) = ldftn($Invoke15);
			*(data + 533620) = ldftn($Invoke16);
		}

		public override object CreateInstance()
		{
			return new DeployerController((UIntPtr)0);
		}
	}
}
