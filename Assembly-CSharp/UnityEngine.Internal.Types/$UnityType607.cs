using StaRTS.Externals.Manimal.TransferObjects.Request;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType607 : $UnityType
	{
		public unsafe $UnityType607()
		{
			*(UnityEngine.Internal.$Metadata.data + 505228) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new DefaultRequest((UIntPtr)0);
		}
	}
}
