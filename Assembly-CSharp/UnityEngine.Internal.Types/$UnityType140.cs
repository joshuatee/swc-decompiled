using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType140 : $UnityType
	{
		public unsafe $UnityType140()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 382084) = ldftn($Invoke0);
			*(data + 382112) = ldftn($Invoke1);
			*(data + 382140) = ldftn($Invoke2);
			*(data + 382168) = ldftn($Invoke3);
			*(data + 382196) = ldftn($Invoke4);
			*(data + 382224) = ldftn($Invoke5);
			*(data + 382252) = ldftn($Invoke6);
			*(data + 382280) = ldftn($Invoke7);
			*(data + 382308) = ldftn($Invoke8);
			*(data + 1525800) = ldftn($Get0);
			*(data + 1525804) = ldftn($Set0);
		}

		public override object CreateInstance()
		{
			return new UIDragCamera((UIntPtr)0);
		}
	}
}
