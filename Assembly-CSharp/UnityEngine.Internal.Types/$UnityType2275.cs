using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2275 : $UnityType
	{
		public unsafe $UnityType2275()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 906720) = ldftn($Invoke0);
			*(data + 906748) = ldftn($Invoke1);
			*(data + 906776) = ldftn($Invoke2);
			*(data + 906804) = ldftn($Invoke3);
			*(data + 906832) = ldftn($Invoke4);
			*(data + 906860) = ldftn($Invoke5);
			*(data + 906888) = ldftn($Invoke6);
			*(data + 906916) = ldftn($Invoke7);
			*(data + 906944) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new ActivePerksInfoScreen((UIntPtr)0);
		}
	}
}
