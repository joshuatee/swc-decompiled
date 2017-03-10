using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1919 : $UnityType
	{
		public unsafe $UnityType1919()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 752076) = ldftn($Invoke0);
			*(data + 752104) = ldftn($Invoke1);
			*(data + 752132) = ldftn($Invoke2);
			*(data + 752160) = ldftn($Invoke3);
			*(data + 752188) = ldftn($Invoke4);
			*(data + 752216) = ldftn($Invoke5);
			*(data + 752244) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new GameConstantsVO((UIntPtr)0);
		}
	}
}
