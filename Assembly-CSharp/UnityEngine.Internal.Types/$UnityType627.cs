using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType627 : $UnityType
	{
		public unsafe $UnityType627()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 509064) = ldftn($Invoke0);
			*(data + 509092) = ldftn($Invoke1);
			*(data + 509120) = ldftn($Invoke2);
			*(data + 509148) = ldftn($Invoke3);
			*(data + 509176) = ldftn($Invoke4);
			*(data + 509204) = ldftn($Invoke5);
			*(data + 509232) = ldftn($Invoke6);
			*(data + 509260) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new EmitterPoolCookie((UIntPtr)0);
		}
	}
}
