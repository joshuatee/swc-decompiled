using StaRTS.Main.Models.Commands.Player.Raids;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1525 : $UnityType
	{
		public unsafe $UnityType1525()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 658472) = ldftn($Invoke0);
			*(data + 658500) = ldftn($Invoke1);
			*(data + 658528) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new RaidDefenseStartResponse((UIntPtr)0);
		}
	}
}
