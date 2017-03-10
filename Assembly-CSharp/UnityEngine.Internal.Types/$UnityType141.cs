using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType141 : $UnityType
	{
		public unsafe $UnityType141()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 382336) = ldftn($Invoke0);
			*(data + 382364) = ldftn($Invoke1);
			*(data + 382392) = ldftn($Invoke2);
			*(data + 382420) = ldftn($Invoke3);
			*(data + 382448) = ldftn($Invoke4);
			*(data + 382476) = ldftn($Invoke5);
			*(data + 1525816) = ldftn($Get0);
			*(data + 1525820) = ldftn($Set0);
		}

		public override object CreateInstance()
		{
			return new UIDragDropContainer((UIntPtr)0);
		}
	}
}
