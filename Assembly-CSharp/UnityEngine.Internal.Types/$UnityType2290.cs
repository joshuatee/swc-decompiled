using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2290 : $UnityType
	{
		public unsafe $UnityType2290()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 912124) = ldftn($Invoke0);
			*(data + 912152) = ldftn($Invoke1);
			*(data + 912180) = ldftn($Invoke2);
			*(data + 912208) = ldftn($Invoke3);
			*(data + 912236) = ldftn($Invoke4);
			*(data + 912264) = ldftn($Invoke5);
			*(data + 912292) = ldftn($Invoke6);
			*(data + 912320) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new ClosableScreen((UIntPtr)0);
		}
	}
}
