using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1770 : $UnityType
	{
		public unsafe $UnityType1770()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 680284) = ldftn($Invoke0);
			*(data + 680312) = ldftn($Invoke1);
			*(data + 680340) = ldftn($Invoke2);
			*(data + 680368) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new HQNode((UIntPtr)0);
		}
	}
}
