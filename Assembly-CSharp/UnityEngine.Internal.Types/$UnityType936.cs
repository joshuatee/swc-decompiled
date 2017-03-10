using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType936 : $UnityType
	{
		public unsafe $UnityType936()
		{
			*(UnityEngine.Internal.$Metadata.data + 597068) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new SchedulingStartupTask((UIntPtr)0);
		}
	}
}
