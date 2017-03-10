using StaRTS.Main.Controllers.Entities.Systems;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType814 : $UnityType
	{
		public unsafe $UnityType814()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 571952) = ldftn($Invoke0);
			*(data + 571980) = ldftn($Invoke1);
			*(data + 572008) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new GeneratorSystem((UIntPtr)0);
		}
	}
}
