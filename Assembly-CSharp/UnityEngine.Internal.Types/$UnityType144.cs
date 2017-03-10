using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType144 : $UnityType
	{
		public unsafe $UnityType144()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 383120) = ldftn($Invoke0);
			*(data + 383148) = ldftn($Invoke1);
			*(data + 383176) = ldftn($Invoke2);
			*(data + 383204) = ldftn($Invoke3);
			*(data + 383232) = ldftn($Invoke4);
			*(data + 383260) = ldftn($Invoke5);
			*(data + 383288) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new UIDragDropRoot((UIntPtr)0);
		}
	}
}
