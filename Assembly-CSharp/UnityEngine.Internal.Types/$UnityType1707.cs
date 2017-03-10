using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1707 : $UnityType
	{
		public unsafe $UnityType1707()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 669784) = ldftn($Invoke0);
			*(data + 669812) = ldftn($Invoke1);
			*(data + 669840) = ldftn($Invoke2);
			*(data + 669868) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new GeneratorComponent((UIntPtr)0);
		}
	}
}
