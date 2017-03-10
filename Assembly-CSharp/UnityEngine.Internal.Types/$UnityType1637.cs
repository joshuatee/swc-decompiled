using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1637 : $UnityType
	{
		public unsafe $UnityType1637()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 662084) = ldftn($Invoke0);
			*(data + 662112) = ldftn($Invoke1);
			*(data + 662140) = ldftn($Invoke2);
			*(data + 662168) = ldftn($Invoke3);
			*(data + 662196) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new ShareVideoRequest((UIntPtr)0);
		}
	}
}
