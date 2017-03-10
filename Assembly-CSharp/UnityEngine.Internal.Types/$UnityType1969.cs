using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1969 : $UnityType
	{
		public unsafe $UnityType1969()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 803932) = ldftn($Invoke0);
			*(data + 803960) = ldftn($Invoke1);
			*(data + 803988) = ldftn($Invoke2);
			*(data + 804016) = ldftn($Invoke3);
			*(data + 804044) = ldftn($Invoke4);
			*(data + 804072) = ldftn($Invoke5);
			*(data + 804100) = ldftn($Invoke6);
			*(data + 804128) = ldftn($Invoke7);
			*(data + 804156) = ldftn($Invoke8);
			*(data + 804184) = ldftn($Invoke9);
			*(data + 804212) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new TextureVO((UIntPtr)0);
		}
	}
}
