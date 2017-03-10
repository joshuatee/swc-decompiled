using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType706 : $UnityType
	{
		public unsafe $UnityType706()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 534068) = ldftn($Invoke0);
			*(data + 534096) = ldftn($Invoke1);
			*(data + 534124) = ldftn($Invoke2);
			*(data + 534152) = ldftn($Invoke3);
			*(data + 534180) = ldftn($Invoke4);
			*(data + 534208) = ldftn($Invoke5);
			*(data + 534236) = ldftn($Invoke6);
			*(data + 534264) = ldftn($Invoke7);
			*(data + 534292) = ldftn($Invoke8);
			*(data + 534320) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new EditBaseController((UIntPtr)0);
		}
	}
}
