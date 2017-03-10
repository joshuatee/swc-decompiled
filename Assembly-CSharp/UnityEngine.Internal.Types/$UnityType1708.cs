using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1708 : $UnityType
	{
		public unsafe $UnityType1708()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 669896) = ldftn($Invoke0);
			*(data + 669924) = ldftn($Invoke1);
			*(data + 669952) = ldftn($Invoke2);
			*(data + 669980) = ldftn($Invoke3);
			*(data + 670008) = ldftn($Invoke4);
			*(data + 670036) = ldftn($Invoke5);
			*(data + 670064) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new GeneratorViewComponent((UIntPtr)0);
		}
	}
}
