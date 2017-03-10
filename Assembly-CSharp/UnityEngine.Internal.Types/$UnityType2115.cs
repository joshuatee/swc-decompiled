using StaRTS.Main.Utils;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2115 : $UnityType
	{
		public unsafe $UnityType2115()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 848200) = ldftn($Invoke0);
			*(data + 848228) = ldftn($Invoke1);
			*(data + 848256) = ldftn($Invoke2);
			*(data + 848284) = ldftn($Invoke3);
			*(data + 848312) = ldftn($Invoke4);
			*(data + 848340) = ldftn($Invoke5);
			*(data + 848368) = ldftn($Invoke6);
			*(data + 848396) = ldftn($Invoke7);
			*(data + 848424) = ldftn($Invoke8);
			*(data + 848452) = ldftn($Invoke9);
			*(data + 848480) = ldftn($Invoke10);
			*(data + 848508) = ldftn($Invoke11);
			*(data + 848536) = ldftn($Invoke12);
			*(data + 848564) = ldftn($Invoke13);
			*(data + 848592) = ldftn($Invoke14);
			*(data + 848620) = ldftn($Invoke15);
			*(data + 848648) = ldftn($Invoke16);
			*(data + 848676) = ldftn($Invoke17);
		}

		public override object CreateInstance()
		{
			return new FactionIconUpgradeController((UIntPtr)0);
		}
	}
}
