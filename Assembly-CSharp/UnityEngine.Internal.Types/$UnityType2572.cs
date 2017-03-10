using StaRTS.Utils.MeshCombiner;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2572 : $UnityType
	{
		public unsafe $UnityType2572()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 996936) = ldftn($Invoke0);
			*(data + 996964) = ldftn($Invoke1);
			*(data + 996992) = ldftn($Invoke2);
			*(data + 997020) = ldftn($Invoke3);
			*(data + 997048) = ldftn($Invoke4);
			*(data + 997076) = ldftn($Invoke5);
			*(data + 997104) = ldftn($Invoke6);
			*(data + 997132) = ldftn($Invoke7);
			*(data + 997160) = ldftn($Invoke8);
			*(data + 997188) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new MeshCombiner((UIntPtr)0);
		}
	}
}
