using StaRTS.Main.Views.Projectors;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2201 : $UnityType
	{
		public unsafe $UnityType2201()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 871076) = ldftn($Invoke0);
			*(data + 871104) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ProjectorForceReloadHelper((UIntPtr)0);
		}
	}
}
