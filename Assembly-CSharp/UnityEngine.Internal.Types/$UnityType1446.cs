using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1446 : $UnityType
	{
		public unsafe $UnityType1446()
		{
			*(UnityEngine.Internal.$Metadata.data + 654048) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new KeepAliveRequest((UIntPtr)0);
		}
	}
}
