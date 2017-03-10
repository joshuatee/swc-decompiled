using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType640 : $UnityType
	{
		public unsafe $UnityType640()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 512116) = ldftn($Invoke0);
			*(data + 512144) = ldftn($Invoke1);
			*(data + 512172) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new ShieldDissolve((UIntPtr)0);
		}
	}
}
