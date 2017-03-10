using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType714 : $UnityType
	{
		public unsafe $UnityType714()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 536196) = ldftn($Invoke0);
			*(data + 536224) = ldftn($Invoke1);
			*(data + 536252) = ldftn($Invoke2);
			*(data + 536280) = ldftn($Invoke3);
			*(data + 536308) = ldftn($Invoke4);
			*(data + 536336) = ldftn($Invoke5);
			*(data + 536364) = ldftn($Invoke6);
			*(data + 536392) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new HoloController((UIntPtr)0);
		}
	}
}
