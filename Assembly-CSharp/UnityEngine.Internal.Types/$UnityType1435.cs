using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1435 : $UnityType
	{
		public unsafe $UnityType1435()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 653404) = ldftn($Invoke0);
			*(data + 653432) = ldftn($Invoke1);
			*(data + 653460) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new DeregisterDeviceRequest((UIntPtr)0);
		}
	}
}
