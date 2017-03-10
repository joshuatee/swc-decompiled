using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType782 : $UnityType
	{
		public unsafe $UnityType782()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 565540) = ldftn($Invoke0);
			*(data + 565568) = ldftn($Invoke1);
			*(data + 565596) = ldftn($Invoke2);
			*(data + 565624) = ldftn($Invoke3);
			*(data + 565652) = ldftn($Invoke4);
			*(data + 565680) = ldftn($Invoke5);
			*(data + 565708) = ldftn($Invoke6);
			*(data + 565736) = ldftn($Invoke7);
			*(data + 565764) = ldftn($Invoke8);
			*(data + 565792) = ldftn($Invoke9);
			*(data + 565820) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new TurretAttackController((UIntPtr)0);
		}
	}
}
