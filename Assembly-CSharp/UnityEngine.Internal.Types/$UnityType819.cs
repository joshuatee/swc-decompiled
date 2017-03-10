using StaRTS.Main.Controllers.Entities.Systems;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType819 : $UnityType
	{
		public unsafe $UnityType819()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 572456) = ldftn($Invoke0);
			*(data + 572484) = ldftn($Invoke1);
			*(data + 572512) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new TargetingSystem((UIntPtr)0);
		}
	}
}
