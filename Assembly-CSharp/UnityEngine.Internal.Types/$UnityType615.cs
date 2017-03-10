using StaRTS.Externals.Manimal.TransferObjects.Response;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType615 : $UnityType
	{
		public unsafe $UnityType615()
		{
			*(UnityEngine.Internal.$Metadata.data + 505760) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new DefaultResponse((UIntPtr)0);
		}
	}
}
