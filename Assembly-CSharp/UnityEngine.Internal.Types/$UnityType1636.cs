using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1636 : $UnityType
	{
		public unsafe $UnityType1636()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 661944) = ldftn($Invoke0);
			*(data + 661972) = ldftn($Invoke1);
			*(data + 662000) = ldftn($Invoke2);
			*(data + 662028) = ldftn($Invoke3);
			*(data + 662056) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new ShareReplayRequest((UIntPtr)0);
		}
	}
}
