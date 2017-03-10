using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType83 : $UnityType
	{
		public unsafe $UnityType83()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 362372) = ldftn($Invoke0);
			*(data + 362400) = ldftn($Invoke1);
			*(data + 362428) = ldftn($Invoke2);
			*(data + 362456) = ldftn($Invoke3);
			*(data + 362484) = ldftn($Invoke4);
			*(data + 362512) = ldftn($Invoke5);
			*(data + 362540) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new StorageEffects((UIntPtr)0);
		}
	}
}
