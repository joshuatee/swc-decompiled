using StaRTS.Main.Models.Static;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1888 : $UnityType
	{
		public unsafe $UnityType1888()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 707976) = ldftn($Invoke0);
			*(data + 708004) = ldftn($Invoke1);
			*(data + 708032) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new StarshipUpgradeCatalog((UIntPtr)0);
		}
	}
}
