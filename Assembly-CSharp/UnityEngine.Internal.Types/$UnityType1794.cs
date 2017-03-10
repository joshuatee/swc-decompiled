using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1794 : $UnityType
	{
		public unsafe $UnityType1794()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 684820) = ldftn($Invoke0);
			*(data + 684848) = ldftn($Invoke1);
			*(data + 684876) = ldftn($Invoke2);
			*(data + 684904) = ldftn($Invoke3);
			*(data + 684932) = ldftn($Invoke4);
			*(data + 684960) = ldftn($Invoke5);
			*(data + 684988) = ldftn($Invoke6);
			*(data + 685016) = ldftn($Invoke7);
			*(data + 685044) = ldftn($Invoke8);
			*(data + 685072) = ldftn($Invoke9);
			*(data + 685100) = ldftn($Invoke10);
			*(data + 685128) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new TurretNode((UIntPtr)0);
		}
	}
}
