using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType637 : $UnityType
	{
		public unsafe $UnityType637()
		{
			*(UnityEngine.Internal.$Metadata.data + 511276) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new ShaderTimeController((UIntPtr)0);
		}
	}
}
