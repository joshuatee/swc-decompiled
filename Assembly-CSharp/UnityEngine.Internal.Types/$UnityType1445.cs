using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1445 : $UnityType
	{
		public unsafe $UnityType1445()
		{
			*(UnityEngine.Internal.$Metadata.data + 654020) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new KeepAliveCommand((UIntPtr)0);
		}
	}
}
