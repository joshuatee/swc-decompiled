using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType925 : $UnityType
	{
		public unsafe $UnityType925()
		{
			*(UnityEngine.Internal.$Metadata.data + 596396) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new ExternalsStartupTask((UIntPtr)0);
		}
	}
}
