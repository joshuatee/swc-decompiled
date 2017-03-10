using StaRTS.Main.Views.Entities;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2185 : $UnityType
	{
		public unsafe $UnityType2185()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 869088) = ldftn($Invoke0);
			*(data + 869116) = ldftn($Invoke1);
			*(data + 869144) = ldftn($Invoke2);
			*(data + 869172) = ldftn($Invoke3);
			*(data + 869200) = ldftn($Invoke4);
			*(data + 869228) = ldftn($Invoke5);
			*(data + 869256) = ldftn($Invoke6);
			*(data + 869284) = ldftn($Invoke7);
			*(data + 869312) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new ShaderSwappedAsset((UIntPtr)0);
		}
	}
}
