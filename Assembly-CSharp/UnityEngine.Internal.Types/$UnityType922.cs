using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType922 : $UnityType
	{
		public unsafe $UnityType922()
		{
			*(UnityEngine.Internal.$Metadata.data + 596284) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new DonePreloadingStartupTask((UIntPtr)0);
		}
	}
}
