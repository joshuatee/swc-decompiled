using StaRTS.Main.Views.Projectors;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2202 : $UnityType
	{
		public unsafe $UnityType2202()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 871132) = ldftn($Invoke0);
			*(data + 871160) = ldftn($Invoke1);
			*(data + 871188) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new ProjectorManager((UIntPtr)0);
		}
	}
}
