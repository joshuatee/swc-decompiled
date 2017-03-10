using MD5;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType247 : $UnityType
	{
		public unsafe $UnityType247()
		{
			*(UnityEngine.Internal.$Metadata.data + 416188) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new Digest((UIntPtr)0);
		}
	}
}
