using StaRTS.Main.Controllers.Missions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType851 : $UnityType
	{
		public unsafe $UnityType851()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 577496) = ldftn($Invoke0);
			*(data + 577524) = ldftn($Invoke1);
			*(data + 577552) = ldftn($Invoke2);
			*(data + 577580) = ldftn($Invoke3);
			*(data + 577608) = ldftn($Invoke4);
			*(data + 577636) = ldftn($Invoke5);
			*(data + 577664) = ldftn($Invoke6);
			*(data + 577692) = ldftn($Invoke7);
			*(data + 577720) = ldftn($Invoke8);
			*(data + 577748) = ldftn($Invoke9);
			*(data + 577776) = ldftn($Invoke10);
			*(data + 577804) = ldftn($Invoke11);
			*(data + 577832) = ldftn($Invoke12);
			*(data + 577860) = ldftn($Invoke13);
		}

		public override object CreateInstance()
		{
			return new MissionConductor((UIntPtr)0);
		}
	}
}
