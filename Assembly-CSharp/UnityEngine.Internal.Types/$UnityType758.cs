using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType758 : $UnityType
	{
		public unsafe $UnityType758()
		{
			*(UnityEngine.Internal.$Metadata.data + 551092) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new ServerController((UIntPtr)0);
		}
	}
}
