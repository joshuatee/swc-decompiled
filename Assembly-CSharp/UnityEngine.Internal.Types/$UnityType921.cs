using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType921 : $UnityType
	{
		public unsafe $UnityType921()
		{
			*(UnityEngine.Internal.$Metadata.data + 596256) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new DamageStartupTask((UIntPtr)0);
		}
	}
}
