using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1736 : $UnityType
	{
		public unsafe $UnityType1736()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 674572) = ldftn($Invoke0);
			*(data + 674600) = ldftn($Invoke1);
			*(data + 674628) = ldftn($Invoke2);
			*(data + 674656) = ldftn($Invoke3);
			*(data + 674684) = ldftn($Invoke4);
			*(data + 674712) = ldftn($Invoke5);
			*(data + 674740) = ldftn($Invoke6);
			*(data + 674768) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new TrackingGameObjectViewComponent((UIntPtr)0);
		}
	}
}
