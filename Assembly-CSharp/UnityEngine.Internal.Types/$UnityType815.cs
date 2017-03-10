using StaRTS.Main.Controllers.Entities.Systems;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType815 : $UnityType
	{
		public unsafe $UnityType815()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 572036) = ldftn($Invoke0);
			*(data + 572064) = ldftn($Invoke1);
			*(data + 572092) = ldftn($Invoke2);
			*(data + 572120) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new HealerTargetingSystem((UIntPtr)0);
		}
	}
}
