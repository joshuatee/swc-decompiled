using StaRTS.Main.Views.Projectors;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2206 : $UnityType
	{
		public unsafe $UnityType2206()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 871664) = ldftn($Invoke0);
			*(data + 871692) = ldftn($Invoke1);
			*(data + 871720) = ldftn($Invoke2);
			*(data + 871748) = ldftn($Invoke3);
			*(data + 871776) = ldftn($Invoke4);
			*(data + 871804) = ldftn($Invoke5);
			*(data + 871832) = ldftn($Invoke6);
			*(data + 871860) = ldftn($Invoke7);
			*(data + 871888) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new ProjectorShaderSwapDecorator((UIntPtr)0);
		}
	}
}
