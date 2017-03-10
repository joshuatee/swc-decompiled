using StaRTS.Main.Models.Commands.Player.Account.External;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1479 : $UnityType
	{
		public unsafe $UnityType1479()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 656148) = ldftn($Invoke0);
			*(data + 656176) = ldftn($Invoke1);
			*(data + 656204) = ldftn($Invoke2);
			*(data + 656232) = ldftn($Invoke3);
			*(data + 656260) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new UnregisterExternalAccountRequest((UIntPtr)0);
		}
	}
}
