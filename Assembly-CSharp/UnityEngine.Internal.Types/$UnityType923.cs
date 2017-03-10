using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType923 : $UnityType
	{
		public unsafe $UnityType923()
		{
			*(UnityEngine.Internal.$Metadata.data + 596312) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new EditBaseStartupTask((UIntPtr)0);
		}
	}
}
