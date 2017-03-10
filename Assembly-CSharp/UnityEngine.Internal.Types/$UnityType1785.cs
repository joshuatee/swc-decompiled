using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1785 : $UnityType
	{
		public unsafe $UnityType1785()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 683028) = ldftn($Invoke0);
			*(data + 683056) = ldftn($Invoke1);
			*(data + 683084) = ldftn($Invoke2);
			*(data + 683112) = ldftn($Invoke3);
			*(data + 683140) = ldftn($Invoke4);
			*(data + 683168) = ldftn($Invoke5);
			*(data + 683196) = ldftn($Invoke6);
			*(data + 683224) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new SupportViewNode((UIntPtr)0);
		}
	}
}
