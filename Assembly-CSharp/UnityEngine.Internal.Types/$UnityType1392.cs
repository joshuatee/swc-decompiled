using StaRTS.Main.Models.Commands.Crates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1392 : $UnityType
	{
		public unsafe $UnityType1392()
		{
			*(UnityEngine.Internal.$Metadata.data + 652004) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new OpenCrateRequest((UIntPtr)0);
		}
	}
}
