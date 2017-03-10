using StaRTS.Main.Controllers.Missions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType850 : $UnityType
	{
		public unsafe $UnityType850()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 577216) = ldftn($Invoke0);
			*(data + 577244) = ldftn($Invoke1);
			*(data + 577272) = ldftn($Invoke2);
			*(data + 577300) = ldftn($Invoke3);
			*(data + 577328) = ldftn($Invoke4);
			*(data + 577356) = ldftn($Invoke5);
			*(data + 577384) = ldftn($Invoke6);
			*(data + 577412) = ldftn($Invoke7);
			*(data + 577440) = ldftn($Invoke8);
			*(data + 577468) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new DefensiveCombatMissionProcessor((UIntPtr)0);
		}
	}
}
