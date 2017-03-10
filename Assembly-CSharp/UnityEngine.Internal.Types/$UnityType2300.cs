using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2300 : $UnityType
	{
		public unsafe $UnityType2300()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 916296) = ldftn($Invoke0);
			*(data + 916324) = ldftn($Invoke1);
			*(data + 916352) = ldftn($Invoke2);
			*(data + 916380) = ldftn($Invoke3);
			*(data + 916408) = ldftn($Invoke4);
			*(data + 916436) = ldftn($Invoke5);
			*(data + 916464) = ldftn($Invoke6);
			*(data + 916492) = ldftn($Invoke7);
			*(data + 916520) = ldftn($Invoke8);
			*(data + 916548) = ldftn($Invoke9);
			*(data + 916576) = ldftn($Invoke10);
			*(data + 916604) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new EquipmentUnlockedCelebrationScreen((UIntPtr)0);
		}
	}
}
