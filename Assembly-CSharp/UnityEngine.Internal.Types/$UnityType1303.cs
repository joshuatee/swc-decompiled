using StaRTS.Main.Models.Commands;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1303 : $UnityType
	{
		public unsafe $UnityType1303()
		{
			*(UnityEngine.Internal.$Metadata.data + 649288) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new GetReplayRequest((UIntPtr)0);
		}
	}
}
