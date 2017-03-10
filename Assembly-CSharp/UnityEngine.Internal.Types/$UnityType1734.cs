using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1734 : $UnityType
	{
		public unsafe $UnityType1734()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 673788) = ldftn($Invoke0);
			*(data + 673816) = ldftn($Invoke1);
			*(data + 673844) = ldftn($Invoke2);
			*(data + 673872) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new TeamComponent((UIntPtr)0);
		}
	}
}
