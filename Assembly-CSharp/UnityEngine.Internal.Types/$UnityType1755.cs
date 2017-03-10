using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1755 : $UnityType
	{
		public unsafe $UnityType1755()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 677344) = ldftn($Invoke0);
			*(data + 677372) = ldftn($Invoke1);
			*(data + 677400) = ldftn($Invoke2);
			*(data + 677428) = ldftn($Invoke3);
			*(data + 677456) = ldftn($Invoke4);
			*(data + 677484) = ldftn($Invoke5);
			*(data + 677512) = ldftn($Invoke6);
			*(data + 677540) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new BuildingRenderNode((UIntPtr)0);
		}
	}
}
