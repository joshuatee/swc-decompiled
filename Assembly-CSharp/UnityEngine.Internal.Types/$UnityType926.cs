using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType926 : $UnityType
	{
		public unsafe $UnityType926()
		{
			*(UnityEngine.Internal.$Metadata.data + 596424) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new GameDataStartupTask((UIntPtr)0);
		}
	}
}
