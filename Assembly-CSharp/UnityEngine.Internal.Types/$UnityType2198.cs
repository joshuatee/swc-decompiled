using StaRTS.Main.Views.Projectors;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2198 : $UnityType
	{
		public unsafe $UnityType2198()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 870684) = ldftn($Invoke0);
			*(data + 870712) = ldftn($Invoke1);
			*(data + 870740) = ldftn($Invoke2);
			*(data + 870768) = ldftn($Invoke3);
			*(data + 870796) = ldftn($Invoke4);
			*(data + 870824) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new ProjectorAssetProcessor((UIntPtr)0);
		}
	}
}
