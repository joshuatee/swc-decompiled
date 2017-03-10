using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType61 : $UnityType
	{
		public unsafe $UnityType61()
		{
			*(UnityEngine.Internal.$Metadata.data + 359292) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new OSUtility((UIntPtr)0);
		}
	}
}
