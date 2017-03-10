using StaRTS.Main.Controllers.Missions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType853 : $UnityType
	{
		public unsafe $UnityType853()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 577916) = ldftn($Invoke0);
			*(data + 577944) = ldftn($Invoke1);
			*(data + 577972) = ldftn($Invoke2);
			*(data + 578000) = ldftn($Invoke3);
			*(data + 578028) = ldftn($Invoke4);
			*(data + 578056) = ldftn($Invoke5);
			*(data + 578084) = ldftn($Invoke6);
			*(data + 578112) = ldftn($Invoke7);
			*(data + 578140) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new MultiCombatEventMissionProcessor((UIntPtr)0);
		}
	}
}
