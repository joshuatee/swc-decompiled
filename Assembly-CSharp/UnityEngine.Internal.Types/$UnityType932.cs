using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType932 : $UnityType
	{
		public unsafe $UnityType932()
		{
			*(UnityEngine.Internal.$Metadata.data + 596844) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new PlayerIdentityStartupTask((UIntPtr)0);
		}
	}
}
