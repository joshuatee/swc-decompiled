using StaRTS.Main.Controllers.Missions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType854 : $UnityType
	{
		public unsafe $UnityType854()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 578168) = ldftn($Invoke0);
			*(data + 578196) = ldftn($Invoke1);
			*(data + 578224) = ldftn($Invoke2);
			*(data + 578252) = ldftn($Invoke3);
			*(data + 578280) = ldftn($Invoke4);
			*(data + 578308) = ldftn($Invoke5);
			*(data + 578336) = ldftn($Invoke6);
			*(data + 578364) = ldftn($Invoke7);
			*(data + 578392) = ldftn($Invoke8);
			*(data + 578420) = ldftn($Invoke9);
			*(data + 578448) = ldftn($Invoke10);
			*(data + 578476) = ldftn($Invoke11);
			*(data + 578504) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new OffensiveCombatMissionProcessor((UIntPtr)0);
		}
	}
}
