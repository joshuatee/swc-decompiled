using StaRTS.Main.Story;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1995 : $UnityType
	{
		public unsafe $UnityType1995()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 836188) = ldftn($Invoke0);
			*(data + 836216) = ldftn($Invoke1);
			*(data + 836244) = ldftn($Invoke2);
			*(data + 836272) = ldftn($Invoke3);
			*(data + 836300) = ldftn($Invoke4);
			*(data + 836328) = ldftn($Invoke5);
			*(data + 836356) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new ActionChain((UIntPtr)0);
		}
	}
}
