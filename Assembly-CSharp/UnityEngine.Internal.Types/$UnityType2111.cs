using StaRTS.Main.Utils;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2111 : $UnityType
	{
		public unsafe $UnityType2111()
		{
			*(UnityEngine.Internal.$Metadata.data + 846884) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CircleMeshUtils((UIntPtr)0);
		}
	}
}
