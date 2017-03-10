using StaRTS.Main.Views.Projectors;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2207 : $UnityType
	{
		public unsafe $UnityType2207()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 871916) = ldftn($Invoke0);
			*(data + 871944) = ldftn($Invoke1);
			*(data + 871972) = ldftn($Invoke2);
			*(data + 872000) = ldftn($Invoke3);
			*(data + 872028) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new ProjectorTurretDecorator((UIntPtr)0);
		}
	}
}
