using StaRTS.Main.Views.Entities;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2172 : $UnityType
	{
		public unsafe $UnityType2172()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 866820) = ldftn($Invoke0);
			*(data + 866848) = ldftn($Invoke1);
			*(data + 866876) = ldftn($Invoke2);
			*(data + 866904) = ldftn($Invoke3);
			*(data + 866932) = ldftn($Invoke4);
			*(data + 866960) = ldftn($Invoke5);
			*(data + 866988) = ldftn($Invoke6);
			*(data + 867016) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new EntityFlasher((UIntPtr)0);
		}
	}
}
