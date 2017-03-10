using StaRTS.Externals.Manimal.TransferObjects.Response;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType616 : $UnityType
	{
		public unsafe $UnityType616()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 505788) = ldftn($Invoke0);
			*(data + 505816) = ldftn($Invoke1);
			*(data + 505844) = ldftn($Invoke2);
			*(data + 505872) = ldftn($Invoke3);
			*(data + 505900) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new Response((UIntPtr)0);
		}
	}
}
