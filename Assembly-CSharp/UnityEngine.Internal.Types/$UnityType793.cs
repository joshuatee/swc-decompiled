using StaRTS.Main.Controllers.CombatTriggers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType793 : $UnityType
	{
		public unsafe $UnityType793()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 568172) = ldftn($Invoke0);
			*(data + 568200) = ldftn($Invoke1);
			*(data + 568228) = ldftn($Invoke2);
			*(data + 568256) = ldftn($Invoke3);
			*(data + 568284) = ldftn($Invoke4);
			*(data + 568312) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new TrapCombatTrigger((UIntPtr)0);
		}
	}
}
