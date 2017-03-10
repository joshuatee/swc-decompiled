using StaRTS.Main.Views.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2497 : $UnityType
	{
		public unsafe $UnityType2497()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 978624) = ldftn($Invoke0);
			*(data + 978652) = ldftn($Invoke1);
			*(data + 978680) = ldftn($Invoke2);
			*(data + 978708) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new CollectButton((UIntPtr)0);
		}
	}
}
