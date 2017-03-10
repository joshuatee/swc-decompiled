using StaRTS.Main.Views.Projectors;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2195 : $UnityType
	{
		public unsafe $UnityType2195()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 870320) = ldftn($Invoke0);
			*(data + 870348) = ldftn($Invoke1);
			*(data + 870376) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new GeometryProjector((UIntPtr)0);
		}
	}
}
