using StaRTS.Main.Models.Player.Store;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1835 : $UnityType
	{
		public unsafe $UnityType1835()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 701368) = ldftn($Invoke0);
			*(data + 701396) = ldftn($Invoke1);
			*(data + 701424) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new InventoryEntry((UIntPtr)0);
		}
	}
}
