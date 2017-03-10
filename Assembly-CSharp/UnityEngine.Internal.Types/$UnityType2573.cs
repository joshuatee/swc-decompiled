using StaRTS.Utils.MetaData;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2573 : $UnityType
	{
		public unsafe $UnityType2573()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 997216) = ldftn($Invoke0);
			*(data + 997244) = ldftn($Invoke1);
			*(data + 997272) = ldftn($Invoke2);
			*(data + 997300) = ldftn($Invoke3);
			*(data + 997328) = ldftn($Invoke4);
			*(data + 997356) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new Catalog((UIntPtr)0);
		}
	}
}
