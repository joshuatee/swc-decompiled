using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1891 : $UnityType
	{
		public unsafe $UnityType1891()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 708956) = ldftn($Invoke0);
			*(data + 708984) = ldftn($Invoke1);
			*(data + 709012) = ldftn($Invoke2);
			*(data + 709040) = ldftn($Invoke3);
			*(data + 709068) = ldftn($Invoke4);
			*(data + 709096) = ldftn($Invoke5);
			*(data + 709124) = ldftn($Invoke6);
			*(data + 709152) = ldftn($Invoke7);
			*(data + 709180) = ldftn($Invoke8);
			*(data + 709208) = ldftn($Invoke9);
			*(data + 709236) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new AssetTypeVO((UIntPtr)0);
		}
	}
}
