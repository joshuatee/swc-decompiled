using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType741 : $UnityType
	{
		public unsafe $UnityType741()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 545156) = ldftn($Invoke0);
			*(data + 545184) = ldftn($Invoke1);
			*(data + 545212) = ldftn($Invoke2);
			*(data + 545240) = ldftn($Invoke3);
			*(data + 545268) = ldftn($Invoke4);
			*(data + 545296) = ldftn($Invoke5);
			*(data + 545324) = ldftn($Invoke6);
			*(data + 545352) = ldftn($Invoke7);
			*(data + 545380) = ldftn($Invoke8);
			*(data + 545408) = ldftn($Invoke9);
			*(data + 545436) = ldftn($Invoke10);
			*(data + 545464) = ldftn($Invoke11);
			*(data + 545492) = ldftn($Invoke12);
			*(data + 545520) = ldftn($Invoke13);
			*(data + 545548) = ldftn($Invoke14);
			*(data + 545576) = ldftn($Invoke15);
		}

		public override object CreateInstance()
		{
			return new PostBattleRepairController((UIntPtr)0);
		}
	}
}
