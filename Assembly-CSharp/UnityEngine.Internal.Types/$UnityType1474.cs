using StaRTS.Main.Models.Commands.Player.Account.External;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1474 : $UnityType
	{
		public unsafe $UnityType1474()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 655392) = ldftn($Invoke0);
			*(data + 655420) = ldftn($Invoke1);
			*(data + 655448) = ldftn($Invoke2);
			*(data + 655476) = ldftn($Invoke3);
			*(data + 655504) = ldftn($Invoke4);
			*(data + 655532) = ldftn($Invoke5);
			*(data + 655560) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new GetExternalAccountsResponse((UIntPtr)0);
		}
	}
}
