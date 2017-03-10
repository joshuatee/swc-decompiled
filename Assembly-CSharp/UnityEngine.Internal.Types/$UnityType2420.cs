using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2420 : $UnityType
	{
		public unsafe $UnityType2420()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 951464) = ldftn($Invoke0);
			*(data + 951492) = ldftn($Invoke1);
			*(data + 951520) = ldftn($Invoke2);
			*(data + 951548) = ldftn($Invoke3);
			*(data + 951576) = ldftn($Invoke4);
			*(data + 951604) = ldftn($Invoke5);
			*(data + 951632) = ldftn($Invoke6);
			*(data + 951660) = ldftn($Invoke7);
			*(data + 951688) = ldftn($Invoke8);
			*(data + 951716) = ldftn($Invoke9);
			*(data + 951744) = ldftn($Invoke10);
			*(data + 951772) = ldftn($Invoke11);
			*(data + 951800) = ldftn($Invoke12);
			*(data + 951828) = ldftn($Invoke13);
			*(data + 951856) = ldftn($Invoke14);
			*(data + 951884) = ldftn($Invoke15);
		}

		public override object CreateInstance()
		{
			return new HolonetAnimationController((UIntPtr)0);
		}
	}
}
