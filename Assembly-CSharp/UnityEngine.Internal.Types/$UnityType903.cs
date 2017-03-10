using StaRTS.Main.Controllers.ServerMessages;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType903 : $UnityType
	{
		public unsafe $UnityType903()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 590096) = ldftn($Invoke0);
			*(data + 590124) = ldftn($Invoke1);
			*(data + 590152) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new KeepAliveMessage((UIntPtr)0);
		}
	}
}
