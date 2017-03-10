using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType709 : $UnityType
	{
		public unsafe $UnityType709()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 534936) = ldftn($Invoke0);
			*(data + 534964) = ldftn($Invoke1);
			*(data + 534992) = ldftn($Invoke2);
			*(data + 535020) = ldftn($Invoke3);
			*(data + 535048) = ldftn($Invoke4);
			*(data + 535076) = ldftn($Invoke5);
			*(data + 535104) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new EntityIdleController((UIntPtr)0);
		}
	}
}
