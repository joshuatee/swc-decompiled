using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1632 : $UnityType
	{
		public unsafe $UnityType1632()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 661664) = ldftn($Invoke0);
			*(data + 661692) = ldftn($Invoke1);
			*(data + 661720) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new MemberIdRequest((UIntPtr)0);
		}
	}
}
