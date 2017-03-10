using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1754 : $UnityType
	{
		public unsafe $UnityType1754()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 677120) = ldftn($Invoke0);
			*(data + 677148) = ldftn($Invoke1);
			*(data + 677176) = ldftn($Invoke2);
			*(data + 677204) = ldftn($Invoke3);
			*(data + 677232) = ldftn($Invoke4);
			*(data + 677260) = ldftn($Invoke5);
			*(data + 677288) = ldftn($Invoke6);
			*(data + 677316) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new BuildingNode((UIntPtr)0);
		}
	}
}
