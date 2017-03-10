using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType684 : $UnityType
	{
		public unsafe $UnityType684()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 522504) = ldftn($Invoke0);
			*(data + 522532) = ldftn($Invoke1);
			*(data + 522560) = ldftn($Invoke2);
			*(data + 522588) = ldftn($Invoke3);
			*(data + 522616) = ldftn($Invoke4);
			*(data + 522644) = ldftn($Invoke5);
			*(data + 522672) = ldftn($Invoke6);
			*(data + 522700) = ldftn($Invoke7);
			*(data + 522728) = ldftn($Invoke8);
			*(data + 522756) = ldftn($Invoke9);
			*(data + 522784) = ldftn($Invoke10);
			*(data + 522812) = ldftn($Invoke11);
			*(data + 522840) = ldftn($Invoke12);
			*(data + 522868) = ldftn($Invoke13);
			*(data + 522896) = ldftn($Invoke14);
			*(data + 522924) = ldftn($Invoke15);
			*(data + 522952) = ldftn($Invoke16);
		}

		public override object CreateInstance()
		{
			return new BattlePlaybackController((UIntPtr)0);
		}
	}
}
