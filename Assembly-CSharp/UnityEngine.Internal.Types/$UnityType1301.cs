using StaRTS.Main.Models.Commands;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1301 : $UnityType
	{
		public unsafe $UnityType1301()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 649036) = ldftn($Invoke0);
			*(data + 649064) = ldftn($Invoke1);
			*(data + 649092) = ldftn($Invoke2);
			*(data + 649120) = ldftn($Invoke3);
			*(data + 649148) = ldftn($Invoke4);
			*(data + 649176) = ldftn($Invoke5);
			*(data + 649204) = ldftn($Invoke6);
			*(data + 649232) = ldftn($Invoke7);
			*(data + 649260) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new GetEndpointsResponse((UIntPtr)0);
		}
	}
}
