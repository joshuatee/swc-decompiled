using StaRTS.Main.Controllers.GameStates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType823 : $UnityType
	{
		public unsafe $UnityType823()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 572764) = ldftn($Invoke0);
			*(data + 572792) = ldftn($Invoke1);
			*(data + 572820) = ldftn($Invoke2);
			*(data + 572848) = ldftn($Invoke3);
			*(data + 572876) = ldftn($Invoke4);
			*(data + 572904) = ldftn($Invoke5);
			*(data + 572932) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new ApplicationLoadState((UIntPtr)0);
		}
	}
}
