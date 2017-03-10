using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1742 : $UnityType
	{
		public unsafe $UnityType1742()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 676112) = ldftn($Invoke0);
			*(data + 676140) = ldftn($Invoke1);
			*(data + 676168) = ldftn($Invoke2);
			*(data + 676196) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new TroopShieldComponent((UIntPtr)0);
		}
	}
}
