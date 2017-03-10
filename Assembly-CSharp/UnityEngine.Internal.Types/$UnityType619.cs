using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType619 : $UnityType
	{
		public unsafe $UnityType619()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 507776) = ldftn($Invoke0);
			*(data + 507804) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new AnimationEventManager((UIntPtr)0);
		}
	}
}
