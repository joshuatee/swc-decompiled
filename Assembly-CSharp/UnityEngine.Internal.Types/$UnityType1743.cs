using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1743 : $UnityType
	{
		public unsafe $UnityType1743()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 676224) = ldftn($Invoke0);
			*(data + 676252) = ldftn($Invoke1);
			*(data + 676280) = ldftn($Invoke2);
			*(data + 676308) = ldftn($Invoke3);
			*(data + 676336) = ldftn($Invoke4);
			*(data + 676364) = ldftn($Invoke5);
			*(data + 676392) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new TroopShieldHealthComponent((UIntPtr)0);
		}
	}
}
