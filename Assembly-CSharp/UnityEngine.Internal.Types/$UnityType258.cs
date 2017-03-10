using Net.RichardLord.Ash.Core;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType258 : $UnityType
	{
		public unsafe $UnityType258()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 417224) = ldftn($Invoke0);
			*(data + 417252) = ldftn($Invoke1);
			*(data + 417280) = ldftn($Invoke2);
			*(data + 417308) = ldftn($Invoke3);
			*(data + 417336) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new EntityList((UIntPtr)0);
		}
	}
}
