using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType942 : $UnityType
	{
		public unsafe $UnityType942()
		{
			*(UnityEngine.Internal.$Metadata.data + 598132) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new TestsStartupTask((UIntPtr)0);
		}
	}
}
