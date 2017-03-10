using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType919 : $UnityType
	{
		public unsafe $UnityType919()
		{
			*(UnityEngine.Internal.$Metadata.data + 596200) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new BIStartupTask((UIntPtr)0);
		}
	}
}
