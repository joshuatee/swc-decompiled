using NGUIExtensions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType253 : $UnityType
	{
		public unsafe $UnityType253()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 416524) = ldftn($Invoke0);
			*(data + 416552) = ldftn($Invoke1);
			*(data + 416580) = ldftn($Invoke2);
			*(data + 416608) = ldftn($Invoke3);
			*(data + 416636) = ldftn($Invoke4);
			*(data + 416664) = ldftn($Invoke5);
			*(data + 1529288) = ldftn($Get0);
			*(data + 1529292) = ldftn($Set0);
			*(data + 1529304) = ldftn($Get1);
			*(data + 1529308) = ldftn($Set1);
			*(data + 1529320) = ldftn($Get2);
			*(data + 1529324) = ldftn($Set2);
		}

		public override object CreateInstance()
		{
			return new ButtonTap((UIntPtr)0);
		}
	}
}
