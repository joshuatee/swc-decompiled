using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType778 : $UnityType
	{
		public unsafe $UnityType778()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 565316) = ldftn($Invoke0);
			*(data + 565344) = ldftn($Invoke1);
			*(data + 565372) = ldftn($Invoke2);
			*(data + 565400) = ldftn($Invoke3);
			*(data + 565428) = ldftn($Invoke4);
			*(data + 565456) = ldftn($Invoke5);
			*(data + 565484) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new TroopController((UIntPtr)0);
		}
	}
}
