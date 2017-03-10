using StaRTS.Main.Models.Player.Store;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1833 : $UnityType
	{
		public unsafe $UnityType1833()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 700864) = ldftn($Invoke0);
			*(data + 700892) = ldftn($Invoke1);
			*(data + 700920) = ldftn($Invoke2);
			*(data + 700948) = ldftn($Invoke3);
			*(data + 700976) = ldftn($Invoke4);
			*(data + 701004) = ldftn($Invoke5);
			*(data + 701032) = ldftn($Invoke6);
			*(data + 701060) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new Inventory((UIntPtr)0);
		}
	}
}
