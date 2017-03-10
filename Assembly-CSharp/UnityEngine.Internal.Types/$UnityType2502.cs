using StaRTS.Main.Views.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2502 : $UnityType
	{
		public unsafe $UnityType2502()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 979296) = ldftn($Invoke0);
			*(data + 979324) = ldftn($Invoke1);
			*(data + 979352) = ldftn($Invoke2);
			*(data + 979380) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new FootprintMesh((UIntPtr)0);
		}
	}
}
