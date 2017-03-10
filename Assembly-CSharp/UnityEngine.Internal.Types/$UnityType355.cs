using StaRTS.Assets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType355 : $UnityType
	{
		public unsafe $UnityType355()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 431308) = ldftn($Invoke0);
			*(data + 431336) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new GameShaders((UIntPtr)0);
		}
	}
}
