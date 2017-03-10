using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1720 : $UnityType
	{
		public unsafe $UnityType1720()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 671520) = ldftn($Invoke0);
			*(data + 671548) = ldftn($Invoke1);
			*(data + 671576) = ldftn($Invoke2);
			*(data + 671604) = ldftn($Invoke3);
			*(data + 671632) = ldftn($Invoke4);
			*(data + 671660) = ldftn($Invoke5);
			*(data + 671688) = ldftn($Invoke6);
			*(data + 671716) = ldftn($Invoke7);
			*(data + 671744) = ldftn($Invoke8);
			*(data + 671772) = ldftn($Invoke9);
			*(data + 671800) = ldftn($Invoke10);
			*(data + 671828) = ldftn($Invoke11);
			*(data + 671856) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new PathingComponent((UIntPtr)0);
		}
	}
}
