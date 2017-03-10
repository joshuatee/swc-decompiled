using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1436 : $UnityType
	{
		public unsafe $UnityType1436()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 653488) = ldftn($Invoke0);
			*(data + 653516) = ldftn($Invoke1);
			*(data + 653544) = ldftn($Invoke2);
			*(data + 653572) = ldftn($Invoke3);
			*(data + 653600) = ldftn($Invoke4);
			*(data + 653628) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new ExternalCurrencySyncResponse((UIntPtr)0);
		}
	}
}
