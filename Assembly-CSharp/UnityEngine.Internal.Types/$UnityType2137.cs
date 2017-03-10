using StaRTS.Main.Utils.Test;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2137 : $UnityType
	{
		public unsafe $UnityType2137()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 859316) = ldftn($Invoke0);
			*(data + 859344) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new PromoCodeTest((UIntPtr)0);
		}
	}
}
