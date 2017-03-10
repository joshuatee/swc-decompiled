using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1701 : $UnityType
	{
		public unsafe $UnityType1701()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 668468) = ldftn($Invoke0);
			*(data + 668496) = ldftn($Invoke1);
			*(data + 668524) = ldftn($Invoke2);
			*(data + 668552) = ldftn($Invoke3);
			*(data + 668580) = ldftn($Invoke4);
			*(data + 668608) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new DroidComponent((UIntPtr)0);
		}
	}
}
