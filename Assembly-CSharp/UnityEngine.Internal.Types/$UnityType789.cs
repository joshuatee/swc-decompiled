using StaRTS.Main.Controllers.CombatTriggers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType789 : $UnityType
	{
		public unsafe $UnityType789()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 567388) = ldftn($Invoke0);
			*(data + 567416) = ldftn($Invoke1);
			*(data + 567444) = ldftn($Invoke2);
			*(data + 567472) = ldftn($Invoke3);
			*(data + 567500) = ldftn($Invoke4);
			*(data + 567528) = ldftn($Invoke5);
			*(data + 567556) = ldftn($Invoke6);
			*(data + 567584) = ldftn($Invoke7);
			*(data + 567612) = ldftn($Invoke8);
			*(data + 567640) = ldftn($Invoke9);
			*(data + 567668) = ldftn($Invoke10);
			*(data + 567696) = ldftn($Invoke11);
			*(data + 567724) = ldftn($Invoke12);
			*(data + 567752) = ldftn($Invoke13);
			*(data + 567780) = ldftn($Invoke14);
			*(data + 567808) = ldftn($Invoke15);
			*(data + 567836) = ldftn($Invoke16);
			*(data + 567864) = ldftn($Invoke17);
		}

		public override object CreateInstance()
		{
			return new DefendedBuildingCombatTrigger((UIntPtr)0);
		}
	}
}
