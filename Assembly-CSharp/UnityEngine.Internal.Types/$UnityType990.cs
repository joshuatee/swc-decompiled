using StaRTS.Main.Controllers.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType990 : $UnityType
	{
		public unsafe $UnityType990()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 604544) = ldftn($Invoke0);
			*(data + 604572) = ldftn($Invoke1);
			*(data + 604600) = ldftn($Invoke2);
			*(data + 604628) = ldftn($Invoke3);
			*(data + 604656) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new WorldPreloader((UIntPtr)0);
		}
	}
}
