using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1787 : $UnityType
	{
		public unsafe $UnityType1787()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 683364) = ldftn($Invoke0);
			*(data + 683392) = ldftn($Invoke1);
			*(data + 683420) = ldftn($Invoke2);
			*(data + 683448) = ldftn($Invoke3);
			*(data + 683476) = ldftn($Invoke4);
			*(data + 683504) = ldftn($Invoke5);
			*(data + 683532) = ldftn($Invoke6);
			*(data + 683560) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new TrackingNode((UIntPtr)0);
		}
	}
}
