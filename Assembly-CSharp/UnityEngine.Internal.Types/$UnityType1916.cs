using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1916 : $UnityType
	{
		public unsafe $UnityType1916()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 747876) = ldftn($Invoke0);
			*(data + 747904) = ldftn($Invoke1);
			*(data + 747932) = ldftn($Invoke2);
			*(data + 747960) = ldftn($Invoke3);
			*(data + 747988) = ldftn($Invoke4);
			*(data + 748016) = ldftn($Invoke5);
			*(data + 748044) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new EncounterProfileVO((UIntPtr)0);
		}
	}
}
