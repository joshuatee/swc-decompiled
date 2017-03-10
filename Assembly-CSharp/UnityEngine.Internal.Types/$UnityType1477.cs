using StaRTS.Main.Models.Commands.Player.Account.External;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1477 : $UnityType
	{
		public unsafe $UnityType1477()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 655952) = ldftn($Invoke0);
			*(data + 655980) = ldftn($Invoke1);
			*(data + 656008) = ldftn($Invoke2);
			*(data + 656036) = ldftn($Invoke3);
			*(data + 656064) = ldftn($Invoke4);
			*(data + 656092) = ldftn($Invoke5);
			*(data + 656120) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new RegisterExternalAccountResponse((UIntPtr)0);
		}
	}
}
