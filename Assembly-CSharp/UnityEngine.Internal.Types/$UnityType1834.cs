using StaRTS.Main.Models.Player.Store;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1834 : $UnityType
	{
		public unsafe $UnityType1834()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 701088) = ldftn($Invoke0);
			*(data + 701116) = ldftn($Invoke1);
			*(data + 701144) = ldftn($Invoke2);
			*(data + 701172) = ldftn($Invoke3);
			*(data + 701200) = ldftn($Invoke4);
			*(data + 701228) = ldftn($Invoke5);
			*(data + 701256) = ldftn($Invoke6);
			*(data + 701284) = ldftn($Invoke7);
			*(data + 701312) = ldftn($Invoke8);
			*(data + 701340) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new InventoryCrates((UIntPtr)0);
		}
	}
}
