using StaRTS.Main.Views.Projectors;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2203 : $UnityType
	{
		public unsafe $UnityType2203()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 871216) = ldftn($Invoke0);
			*(data + 871244) = ldftn($Invoke1);
			*(data + 871272) = ldftn($Invoke2);
			*(data + 871300) = ldftn($Invoke3);
			*(data + 871328) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new ProjectorMeterDecorator((UIntPtr)0);
		}
	}
}
