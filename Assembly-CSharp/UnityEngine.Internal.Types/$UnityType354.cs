using StaRTS.Assets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType354 : $UnityType
	{
		public unsafe $UnityType354()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 431196) = ldftn($Invoke0);
			*(data + 431224) = ldftn($Invoke1);
			*(data + 431252) = ldftn($Invoke2);
			*(data + 431280) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new GameObjectContainer((UIntPtr)0);
		}
	}
}
