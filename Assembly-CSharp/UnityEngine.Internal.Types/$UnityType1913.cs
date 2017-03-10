using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1913 : $UnityType
	{
		public unsafe $UnityType1913()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 746056) = ldftn($Invoke0);
			*(data + 746084) = ldftn($Invoke1);
			*(data + 746112) = ldftn($Invoke2);
			*(data + 746140) = ldftn($Invoke3);
			*(data + 746168) = ldftn($Invoke4);
			*(data + 746196) = ldftn($Invoke5);
			*(data + 746224) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new DefenseEncounterVO((UIntPtr)0);
		}
	}
}
