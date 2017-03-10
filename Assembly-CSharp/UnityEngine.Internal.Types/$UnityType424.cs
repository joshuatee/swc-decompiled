using StaRTS.Externals.Kochava;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType424 : $UnityType
	{
		public unsafe $UnityType424()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 446764) = ldftn($Invoke0);
			*(data + 446792) = ldftn($Invoke1);
			*(data + 446820) = ldftn($Invoke2);
			*(data + 446848) = ldftn($Invoke3);
			*(data + 446876) = ldftn($Invoke4);
			*(data + 446904) = ldftn($Invoke5);
			*(data + 446932) = ldftn($Invoke6);
			*(data + 446960) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new KochavaPlugin((UIntPtr)0);
		}
	}
}
