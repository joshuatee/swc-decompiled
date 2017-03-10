using StaRTS.Main.Views;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2144 : $UnityType
	{
		public unsafe $UnityType2144()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 860408) = ldftn($Invoke0);
			*(data + 860436) = ldftn($Invoke1);
			*(data + 860464) = ldftn($Invoke2);
			*(data + 860492) = ldftn($Invoke3);
			*(data + 860520) = ldftn($Invoke4);
			*(data + 860548) = ldftn($Invoke5);
			*(data + 860576) = ldftn($Invoke6);
			*(data + 860604) = ldftn($Invoke7);
			*(data + 860632) = ldftn($Invoke8);
			*(data + 860660) = ldftn($Invoke9);
			*(data + 860688) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new TransportTroopEffect((UIntPtr)0);
		}
	}
}
