using StaRTS.Main.Controllers.Entities.Systems;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType813 : $UnityType
	{
		public unsafe $UnityType813()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 571868) = ldftn($Invoke0);
			*(data + 571896) = ldftn($Invoke1);
			*(data + 571924) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new EntityRenderSystem((UIntPtr)0);
		}
	}
}
