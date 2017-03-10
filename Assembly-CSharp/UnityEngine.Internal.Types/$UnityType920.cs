using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType920 : $UnityType
	{
		public unsafe $UnityType920()
		{
			*(UnityEngine.Internal.$Metadata.data + 596228) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new BoardStartupTask((UIntPtr)0);
		}
	}
}
