using StaRTS.Main.Controllers.Planets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType896 : $UnityType
	{
		public unsafe $UnityType896()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 588388) = ldftn($Invoke0);
			*(data + 588416) = ldftn($Invoke1);
			*(data + 588444) = ldftn($Invoke2);
			*(data + 588472) = ldftn($Invoke3);
			*(data + 588500) = ldftn($Invoke4);
			*(data + 588528) = ldftn($Invoke5);
			*(data + 588556) = ldftn($Invoke6);
			*(data + 588584) = ldftn($Invoke7);
			*(data + 588612) = ldftn($Invoke8);
			*(data + 588640) = ldftn($Invoke9);
			*(data + 588668) = ldftn($Invoke10);
			*(data + 588696) = ldftn($Invoke11);
			*(data + 588724) = ldftn($Invoke12);
			*(data + 588752) = ldftn($Invoke13);
			*(data + 588780) = ldftn($Invoke14);
			*(data + 588808) = ldftn($Invoke15);
			*(data + 588836) = ldftn($Invoke16);
			*(data + 588864) = ldftn($Invoke17);
			*(data + 588892) = ldftn($Invoke18);
			*(data + 588920) = ldftn($Invoke19);
			*(data + 588948) = ldftn($Invoke20);
		}

		public override object CreateInstance()
		{
			return new PlanetRelocationController((UIntPtr)0);
		}
	}
}
