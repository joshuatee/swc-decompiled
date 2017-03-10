using StaRTS.Main.Views.Projectors;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2208 : $UnityType
	{
		public unsafe $UnityType2208()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 872056) = ldftn($Invoke0);
			*(data + 872084) = ldftn($Invoke1);
			*(data + 872112) = ldftn($Invoke2);
			*(data + 872140) = ldftn($Invoke3);
			*(data + 872168) = ldftn($Invoke4);
			*(data + 872196) = ldftn($Invoke5);
			*(data + 872224) = ldftn($Invoke6);
			*(data + 872252) = ldftn($Invoke7);
			*(data + 872280) = ldftn($Invoke8);
			*(data + 872308) = ldftn($Invoke9);
			*(data + 872336) = ldftn($Invoke10);
			*(data + 872364) = ldftn($Invoke11);
			*(data + 872392) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new ProjectorUtils((UIntPtr)0);
		}
	}
}
