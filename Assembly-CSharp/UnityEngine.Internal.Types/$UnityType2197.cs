using StaRTS.Main.Views.Projectors;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2197 : $UnityType
	{
		public unsafe $UnityType2197()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 870544) = ldftn($Invoke0);
			*(data + 870572) = ldftn($Invoke1);
			*(data + 870600) = ldftn($Invoke2);
			*(data + 870628) = ldftn($Invoke3);
			*(data + 870656) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new ProjectorAnimationDecorator((UIntPtr)0);
		}
	}
}
