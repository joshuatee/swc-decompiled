using StaRTS.Externals.Manimal.TransferObjects.Request;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType612 : $UnityType
	{
		public unsafe $UnityType612()
		{
			*(UnityEngine.Internal.$Metadata.data + 505536) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new PvpGetNextTargetRequest((UIntPtr)0);
		}
	}
}
