using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType620 : $UnityType
	{
		public unsafe $UnityType620()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 507832) = ldftn($Invoke0);
			*(data + 507860) = ldftn($Invoke1);
			*(data + 507888) = ldftn($Invoke2);
			*(data + 507916) = ldftn($Invoke3);
			*(data + 507944) = ldftn($Invoke4);
			*(data + 507972) = ldftn($Invoke5);
			*(data + 508000) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new AnimationEventMonoBehaviour((UIntPtr)0);
		}
	}
}
