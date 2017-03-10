using StaRTS.Main.Views.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2501 : $UnityType
	{
		public unsafe $UnityType2501()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 979100) = ldftn($Invoke0);
			*(data + 979128) = ldftn($Invoke1);
			*(data + 979156) = ldftn($Invoke2);
			*(data + 979184) = ldftn($Invoke3);
			*(data + 979212) = ldftn($Invoke4);
			*(data + 979240) = ldftn($Invoke5);
			*(data + 979268) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new Footprint((UIntPtr)0);
		}
	}
}
