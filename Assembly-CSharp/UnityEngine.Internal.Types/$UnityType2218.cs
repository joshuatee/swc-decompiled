using StaRTS.Main.Views.UX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2218 : $UnityType
	{
		public unsafe $UnityType2218()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 880400) = ldftn($Invoke0);
			*(data + 880428) = ldftn($Invoke1);
			*(data + 880456) = ldftn($Invoke2);
			*(data + 880484) = ldftn($Invoke3);
			*(data + 880512) = ldftn($Invoke4);
			*(data + 880540) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new LabelFader((UIntPtr)0);
		}
	}
}
