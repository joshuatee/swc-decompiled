using StaRTS.Main.Views.Projectors;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2204 : $UnityType
	{
		public unsafe $UnityType2204()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 871356) = ldftn($Invoke0);
			*(data + 871384) = ldftn($Invoke1);
			*(data + 871412) = ldftn($Invoke2);
			*(data + 871440) = ldftn($Invoke3);
			*(data + 871468) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new ProjectorOutlineDecorator((UIntPtr)0);
		}
	}
}
