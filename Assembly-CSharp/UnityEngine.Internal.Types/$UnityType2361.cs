using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2361 : $UnityType
	{
		public unsafe $UnityType2361()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 933908) = ldftn($Invoke0);
			*(data + 933936) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new StorageInfoScreen((UIntPtr)0);
		}
	}
}
