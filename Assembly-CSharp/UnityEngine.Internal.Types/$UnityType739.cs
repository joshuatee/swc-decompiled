using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType739 : $UnityType
	{
		public unsafe $UnityType739()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 544792) = ldftn($Invoke0);
			*(data + 544820) = ldftn($Invoke1);
			*(data + 544848) = ldftn($Invoke2);
			*(data + 544876) = ldftn($Invoke3);
			*(data + 544904) = ldftn($Invoke4);
			*(data + 544932) = ldftn($Invoke5);
			*(data + 544960) = ldftn($Invoke6);
			*(data + 544988) = ldftn($Invoke7);
			*(data + 545016) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new PlayerValuesController((UIntPtr)0);
		}
	}
}
