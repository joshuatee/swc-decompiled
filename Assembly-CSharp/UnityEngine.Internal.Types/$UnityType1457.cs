using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1457 : $UnityType
	{
		public unsafe $UnityType1457()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 655028) = ldftn($Invoke0);
			*(data + 655056) = ldftn($Invoke1);
			*(data + 655084) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new RegisterDeviceRequest((UIntPtr)0);
		}
	}
}
