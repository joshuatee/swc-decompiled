using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType694 : $UnityType
	{
		public unsafe $UnityType694()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 530484) = ldftn($Invoke0);
			*(data + 530512) = ldftn($Invoke1);
			*(data + 530540) = ldftn($Invoke2);
			*(data + 530568) = ldftn($Invoke3);
			*(data + 530596) = ldftn($Invoke4);
			*(data + 530624) = ldftn($Invoke5);
			*(data + 530652) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new CombatEncounterController((UIntPtr)0);
		}
	}
}
