using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1061 : $UnityType
	{
		public unsafe $UnityType1061()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 626580) = ldftn($Invoke0);
			*(data + 626608) = ldftn($Invoke1);
			*(data + 626636) = ldftn($Invoke2);
			*(data + 626664) = ldftn($Invoke3);
			*(data + 626692) = ldftn($Invoke4);
			*(data + 626720) = ldftn($Invoke5);
			*(data + 626748) = ldftn($Invoke6);
			*(data + 626776) = ldftn($Invoke7);
			*(data + 626804) = ldftn($Invoke8);
			*(data + 626832) = ldftn($Invoke9);
			*(data + 626860) = ldftn($Invoke10);
			*(data + 626888) = ldftn($Invoke11);
			*(data + 626916) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new SocialFriendData((UIntPtr)0);
		}
	}
}
