using StaRTS.Main.Views.Projectors;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2210 : $UnityType
	{
		public unsafe $UnityType2210()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 872588) = ldftn($Invoke0);
			*(data + 872616) = ldftn($Invoke1);
			*(data + 872644) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new SurfaceProjectorRenderer((UIntPtr)0);
		}
	}
}
