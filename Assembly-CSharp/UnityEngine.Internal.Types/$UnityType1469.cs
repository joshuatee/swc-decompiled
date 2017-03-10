using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1469 : $UnityType
	{
		public unsafe $UnityType1469()
		{
			*(UnityEngine.Internal.$Metadata.data + 655308) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new TapjoyPlayerSyncResponse((UIntPtr)0);
		}
	}
}
