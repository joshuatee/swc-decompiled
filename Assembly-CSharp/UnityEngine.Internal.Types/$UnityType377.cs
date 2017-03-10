using StaRTS.Externals.BI;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType377 : $UnityType
	{
		public unsafe $UnityType377()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 435844) = ldftn($Invoke0);
			*(data + 435872) = ldftn($Invoke1);
			*(data + 435900) = ldftn($Invoke2);
			*(data + 435928) = ldftn($Invoke3);
			*(data + 435956) = ldftn($Invoke4);
			*(data + 435984) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new Event2LogCreator((UIntPtr)0);
		}
	}
}
