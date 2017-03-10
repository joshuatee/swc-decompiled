using StaRTS.Main.Models.Commands.Player.Identity;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1516 : $UnityType
	{
		public unsafe $UnityType1516()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 658080) = ldftn($Invoke0);
			*(data + 658108) = ldftn($Invoke1);
			*(data + 658136) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new PlayerIdentityGetResponse((UIntPtr)0);
		}
	}
}
