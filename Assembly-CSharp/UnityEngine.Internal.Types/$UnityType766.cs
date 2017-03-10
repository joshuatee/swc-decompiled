using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType766 : $UnityType
	{
		public unsafe $UnityType766()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 555404) = ldftn($Invoke0);
			*(data + 555432) = ldftn($Invoke1);
			*(data + 555460) = ldftn($Invoke2);
			*(data + 555488) = ldftn($Invoke3);
			*(data + 555516) = ldftn($Invoke4);
			*(data + 555544) = ldftn($Invoke5);
			*(data + 555572) = ldftn($Invoke6);
			*(data + 555600) = ldftn($Invoke7);
			*(data + 555628) = ldftn($Invoke8);
			*(data + 555656) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new SquadTroopAttackController((UIntPtr)0);
		}
	}
}
