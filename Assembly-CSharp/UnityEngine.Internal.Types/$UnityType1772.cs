using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1772 : $UnityType
	{
		public unsafe $UnityType1772()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 680452) = ldftn($Invoke0);
			*(data + 680480) = ldftn($Invoke1);
			*(data + 680508) = ldftn($Invoke2);
			*(data + 680536) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new HousingNode((UIntPtr)0);
		}
	}
}
