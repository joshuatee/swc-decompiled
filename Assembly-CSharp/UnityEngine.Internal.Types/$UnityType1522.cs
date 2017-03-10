using StaRTS.Main.Models.Commands.Player.Raids;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1522 : $UnityType
	{
		public unsafe $UnityType1522()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 658360) = ldftn($Invoke0);
			*(data + 658388) = ldftn($Invoke1);
			*(data + 658416) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new RaidDefenseCompleteResponse((UIntPtr)0);
		}
	}
}
