using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1376 : $UnityType
	{
		public unsafe $UnityType1376()
		{
			*(UnityEngine.Internal.$Metadata.data + 651696) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new SimulateWarMatchMakingRequest((UIntPtr)0);
		}
	}
}
