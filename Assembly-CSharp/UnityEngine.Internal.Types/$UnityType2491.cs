using StaRTS.Main.Views.UserInput;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2491 : $UnityType
	{
		public unsafe $UnityType2491()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 975684) = ldftn($Invoke0);
			*(data + 975712) = ldftn($Invoke1);
			*(data + 975740) = ldftn($Invoke2);
			*(data + 975768) = ldftn($Invoke3);
			*(data + 975796) = ldftn($Invoke4);
			*(data + 975824) = ldftn($Invoke5);
			*(data + 975852) = ldftn($Invoke6);
			*(data + 975880) = ldftn($Invoke7);
			*(data + 975908) = ldftn($Invoke8);
			*(data + 975936) = ldftn($Invoke9);
			*(data + 975964) = ldftn($Invoke10);
			*(data + 975992) = ldftn($Invoke11);
			*(data + 976020) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new UserInputInhibitor((UIntPtr)0);
		}
	}
}
