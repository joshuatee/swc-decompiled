using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1738 : $UnityType
	{
		public unsafe $UnityType1738()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 675076) = ldftn($Invoke0);
			*(data + 675104) = ldftn($Invoke1);
			*(data + 675132) = ldftn($Invoke2);
			*(data + 675160) = ldftn($Invoke3);
			*(data + 675188) = ldftn($Invoke4);
			*(data + 675216) = ldftn($Invoke5);
			*(data + 675244) = ldftn($Invoke6);
			*(data + 675272) = ldftn($Invoke7);
			*(data + 675300) = ldftn($Invoke8);
			*(data + 675328) = ldftn($Invoke9);
			*(data + 675356) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new TransportComponent((UIntPtr)0);
		}
	}
}
