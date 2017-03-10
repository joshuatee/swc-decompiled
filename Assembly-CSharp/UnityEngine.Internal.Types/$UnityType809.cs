using StaRTS.Main.Controllers.Entities.Systems;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType809 : $UnityType
	{
		public unsafe $UnityType809()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 571560) = ldftn($Invoke0);
			*(data + 571588) = ldftn($Invoke1);
			*(data + 571616) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new AreaTriggerSystem((UIntPtr)0);
		}
	}
}
