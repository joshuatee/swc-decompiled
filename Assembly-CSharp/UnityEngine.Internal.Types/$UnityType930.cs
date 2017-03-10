using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType930 : $UnityType
	{
		public unsafe $UnityType930()
		{
			*(UnityEngine.Internal.$Metadata.data + 596592) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new PlacementStartupTask((UIntPtr)0);
		}
	}
}
