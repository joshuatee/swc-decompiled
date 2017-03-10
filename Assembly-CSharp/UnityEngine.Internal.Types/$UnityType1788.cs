using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1788 : $UnityType
	{
		public unsafe $UnityType1788()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 683588) = ldftn($Invoke0);
			*(data + 683616) = ldftn($Invoke1);
			*(data + 683644) = ldftn($Invoke2);
			*(data + 683672) = ldftn($Invoke3);
			*(data + 683700) = ldftn($Invoke4);
			*(data + 683728) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new TrackingRenderNode((UIntPtr)0);
		}
	}
}
