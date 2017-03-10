using StaRTS.Main.Views;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2138 : $UnityType
	{
		public unsafe $UnityType2138()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 859372) = ldftn($Invoke0);
			*(data + 859400) = ldftn($Invoke1);
			*(data + 859428) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new DerivedTransformationManager((UIntPtr)0);
		}
	}
}
