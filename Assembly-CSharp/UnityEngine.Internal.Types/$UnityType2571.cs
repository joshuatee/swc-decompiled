using StaRTS.Utils.Json;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2571 : $UnityType
	{
		public unsafe $UnityType2571()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 996628) = ldftn($Invoke0);
			*(data + 996656) = ldftn($Invoke1);
			*(data + 996684) = ldftn($Invoke2);
			*(data + 996712) = ldftn($Invoke3);
			*(data + 996740) = ldftn($Invoke4);
			*(data + 996768) = ldftn($Invoke5);
			*(data + 996796) = ldftn($Invoke6);
			*(data + 996824) = ldftn($Invoke7);
			*(data + 996852) = ldftn($Invoke8);
			*(data + 996880) = ldftn($Invoke9);
			*(data + 996908) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new Serializer((UIntPtr)0);
		}
	}
}
