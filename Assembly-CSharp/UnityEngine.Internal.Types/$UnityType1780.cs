using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1780 : $UnityType
	{
		public unsafe $UnityType1780()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 682356) = ldftn($Invoke0);
			*(data + 682384) = ldftn($Invoke1);
			*(data + 682412) = ldftn($Invoke2);
			*(data + 682440) = ldftn($Invoke3);
			*(data + 682468) = ldftn($Invoke4);
			*(data + 682496) = ldftn($Invoke5);
			*(data + 682524) = ldftn($Invoke6);
			*(data + 682552) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new ShieldGeneratorNode((UIntPtr)0);
		}
	}
}
