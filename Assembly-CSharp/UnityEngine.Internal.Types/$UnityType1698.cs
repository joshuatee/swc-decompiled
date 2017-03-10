using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1698 : $UnityType
	{
		public unsafe $UnityType1698()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 667992) = ldftn($Invoke0);
			*(data + 668020) = ldftn($Invoke1);
			*(data + 668048) = ldftn($Invoke2);
			*(data + 668076) = ldftn($Invoke3);
			*(data + 668104) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new DamageableComponent((UIntPtr)0);
		}
	}
}
