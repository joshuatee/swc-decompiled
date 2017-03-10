using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType768 : $UnityType
	{
		public unsafe $UnityType768()
		{
			*(UnityEngine.Internal.$Metadata.data + 558232) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new StaticDataController((UIntPtr)0);
		}
	}
}
