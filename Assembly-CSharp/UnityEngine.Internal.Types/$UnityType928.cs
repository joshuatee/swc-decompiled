using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType928 : $UnityType
	{
		public unsafe $UnityType928()
		{
			*(UnityEngine.Internal.$Metadata.data + 596508) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new HomeStartupTask((UIntPtr)0);
		}
	}
}
