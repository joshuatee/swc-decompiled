using StaRTS.Main.Controllers.Entities.Systems;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType818 : $UnityType
	{
		public unsafe $UnityType818()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 572372) = ldftn($Invoke0);
			*(data + 572400) = ldftn($Invoke1);
			*(data + 572428) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new SupportViewSystem((UIntPtr)0);
		}
	}
}
