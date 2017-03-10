using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType16 : $UnityType
	{
		public unsafe $UnityType16()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 348904) = ldftn($Invoke0);
			*(data + 348932) = ldftn($Invoke1);
			*(data + 348960) = ldftn($Invoke2);
			*(data + 348988) = ldftn($Invoke3);
			*(data + 349016) = ldftn($Invoke4);
			*(data + 1523960) = ldftn($Get0);
			*(data + 1523964) = ldftn($Set0);
			*(data + 1523976) = ldftn($Get1);
			*(data + 1523980) = ldftn($Set1);
			*(data + 1523992) = ldftn($Get2);
			*(data + 1523996) = ldftn($Set2);
			*(data + 1524008) = ldftn($Get3);
			*(data + 1524012) = ldftn($Set3);
			*(data + 1524024) = ldftn($Get4);
			*(data + 1524028) = ldftn($Set4);
		}

		public override object CreateInstance()
		{
			return new AssetMeshDataMonoBehaviour((UIntPtr)0);
		}
	}
}
