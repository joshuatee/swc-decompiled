using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType679 : $UnityType
	{
		public unsafe $UnityType679()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 518276) = ldftn($Invoke0);
			*(data + 518304) = ldftn($Invoke1);
			*(data + 518332) = ldftn($Invoke2);
			*(data + 518360) = ldftn($Invoke3);
			*(data + 518388) = ldftn($Invoke4);
			*(data + 518416) = ldftn($Invoke5);
			*(data + 518444) = ldftn($Invoke6);
			*(data + 518472) = ldftn($Invoke7);
			*(data + 518500) = ldftn($Invoke8);
			*(data + 518528) = ldftn($Invoke9);
			*(data + 518556) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new AppServerEnvironmentController((UIntPtr)0);
		}
	}
}
