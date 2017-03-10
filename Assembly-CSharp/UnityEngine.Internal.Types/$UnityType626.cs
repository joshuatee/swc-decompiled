using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType626 : $UnityType
	{
		public unsafe $UnityType626()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 508980) = ldftn($Invoke0);
			*(data + 509008) = ldftn($Invoke1);
			*(data + 509036) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new EmitterPool((UIntPtr)0);
		}
	}
}
