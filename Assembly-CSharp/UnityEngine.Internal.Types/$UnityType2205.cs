using StaRTS.Main.Views.Projectors;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2205 : $UnityType
	{
		public unsafe $UnityType2205()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 871496) = ldftn($Invoke0);
			*(data + 871524) = ldftn($Invoke1);
			*(data + 871552) = ldftn($Invoke2);
			*(data + 871580) = ldftn($Invoke3);
			*(data + 871608) = ldftn($Invoke4);
			*(data + 871636) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new ProjectorRotationDecorator((UIntPtr)0);
		}
	}
}
