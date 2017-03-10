using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1705 : $UnityType
	{
		public unsafe $UnityType1705()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 668636) = ldftn($Invoke0);
			*(data + 668664) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new FollowerComponent((UIntPtr)0);
		}
	}
}
