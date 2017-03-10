using StaRTS.Main.Views.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2500 : $UnityType
	{
		public unsafe $UnityType2500()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 979044) = ldftn($Invoke0);
			*(data + 979072) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new EasingDirection((UIntPtr)0);
		}
	}
}
