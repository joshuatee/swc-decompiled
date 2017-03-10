using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType702 : $UnityType
	{
		public unsafe $UnityType702()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 532164) = ldftn($Invoke0);
			*(data + 532192) = ldftn($Invoke1);
			*(data + 532220) = ldftn($Invoke2);
			*(data + 532248) = ldftn($Invoke3);
			*(data + 532276) = ldftn($Invoke4);
			*(data + 532304) = ldftn($Invoke5);
			*(data + 532332) = ldftn($Invoke6);
			*(data + 532360) = ldftn($Invoke7);
			*(data + 532388) = ldftn($Invoke8);
			*(data + 532416) = ldftn($Invoke9);
			*(data + 532444) = ldftn($Invoke10);
			*(data + 532472) = ldftn($Invoke11);
			*(data + 532500) = ldftn($Invoke12);
			*(data + 532528) = ldftn($Invoke13);
			*(data + 532556) = ldftn($Invoke14);
			*(data + 532584) = ldftn($Invoke15);
			*(data + 532612) = ldftn($Invoke16);
			*(data + 532640) = ldftn($Invoke17);
		}

		public override object CreateInstance()
		{
			return new DeployableShardUnlockController((UIntPtr)0);
		}
	}
}
