using StaRTS.Main.Controllers.Entities.Systems;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType816 : $UnityType
	{
		public unsafe $UnityType816()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 572148) = ldftn($Invoke0);
			*(data + 572176) = ldftn($Invoke1);
			*(data + 572204) = ldftn($Invoke2);
			*(data + 572232) = ldftn($Invoke3);
			*(data + 572260) = ldftn($Invoke4);
			*(data + 572288) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new HealthRenderSystem((UIntPtr)0);
		}
	}
}
