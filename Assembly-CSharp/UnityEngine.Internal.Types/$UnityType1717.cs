using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1717 : $UnityType
	{
		public unsafe $UnityType1717()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 671324) = ldftn($Invoke0);
			*(data + 671352) = ldftn($Invoke1);
			*(data + 671380) = ldftn($Invoke2);
			*(data + 671408) = ldftn($Invoke3);
			*(data + 671436) = ldftn($Invoke4);
			*(data + 671464) = ldftn($Invoke5);
			*(data + 671492) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new MeterShaderComponent((UIntPtr)0);
		}
	}
}
