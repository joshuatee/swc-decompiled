using StaRTS.Main.Views.Animations;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2148 : $UnityType
	{
		public unsafe $UnityType2148()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 860912) = ldftn($Invoke0);
			*(data + 860940) = ldftn($Invoke1);
			*(data + 860968) = ldftn($Invoke2);
			*(data + 860996) = ldftn($Invoke3);
			*(data + 861024) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new AnimUXScale((UIntPtr)0);
		}
	}
}
