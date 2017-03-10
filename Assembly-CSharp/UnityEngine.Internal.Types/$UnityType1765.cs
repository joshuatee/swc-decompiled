using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1765 : $UnityType
	{
		public unsafe $UnityType1765()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 679444) = ldftn($Invoke0);
			*(data + 679472) = ldftn($Invoke1);
			*(data + 679500) = ldftn($Invoke2);
			*(data + 679528) = ldftn($Invoke3);
			*(data + 679556) = ldftn($Invoke4);
			*(data + 679584) = ldftn($Invoke5);
			*(data + 679612) = ldftn($Invoke6);
			*(data + 679640) = ldftn($Invoke7);
			*(data + 679668) = ldftn($Invoke8);
			*(data + 679696) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new EntityRenderNode((UIntPtr)0);
		}
	}
}
