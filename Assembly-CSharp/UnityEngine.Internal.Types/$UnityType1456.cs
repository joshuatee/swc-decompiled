using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1456 : $UnityType
	{
		public unsafe $UnityType1456()
		{
			*(UnityEngine.Internal.$Metadata.data + 655000) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new RegisterDeviceCommand((UIntPtr)0);
		}
	}
}
