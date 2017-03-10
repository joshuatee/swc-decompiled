using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType713 : $UnityType
	{
		public unsafe $UnityType713()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 535888) = ldftn($Invoke0);
			*(data + 535916) = ldftn($Invoke1);
			*(data + 535944) = ldftn($Invoke2);
			*(data + 535972) = ldftn($Invoke3);
			*(data + 536000) = ldftn($Invoke4);
			*(data + 536028) = ldftn($Invoke5);
			*(data + 536056) = ldftn($Invoke6);
			*(data + 536084) = ldftn($Invoke7);
			*(data + 536112) = ldftn($Invoke8);
			*(data + 536140) = ldftn($Invoke9);
			*(data + 536168) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new HealthController((UIntPtr)0);
		}
	}
}
