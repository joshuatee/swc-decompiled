using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1692 : $UnityType
	{
		public unsafe $UnityType1692()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 667712) = ldftn($Invoke0);
			*(data + 667740) = ldftn($Invoke1);
			*(data + 667768) = ldftn($Invoke2);
			*(data + 667796) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new BuildingComponent((UIntPtr)0);
		}
	}
}
