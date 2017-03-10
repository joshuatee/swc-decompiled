using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1776 : $UnityType
	{
		public unsafe $UnityType1776()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 681124) = ldftn($Invoke0);
			*(data + 681152) = ldftn($Invoke1);
			*(data + 681180) = ldftn($Invoke2);
			*(data + 681208) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new OffenseLabNode((UIntPtr)0);
		}
	}
}
