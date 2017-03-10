using StaRTS.Main.Controllers.Entities.Systems;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType810 : $UnityType
	{
		public unsafe $UnityType810()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 571644) = ldftn($Invoke0);
			*(data + 571672) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new AttackSystem((UIntPtr)0);
		}
	}
}
