using StaRTS.Main.Controllers.Entities.Systems;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType820 : $UnityType
	{
		public unsafe $UnityType820()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 572540) = ldftn($Invoke0);
			*(data + 572568) = ldftn($Invoke1);
			*(data + 572596) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new TrackingRenderSystem((UIntPtr)0);
		}
	}
}
