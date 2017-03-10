using StaRTS.Main.Views.Entities;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2173 : $UnityType
	{
		public unsafe $UnityType2173()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 867044) = ldftn($Invoke0);
			*(data + 867072) = ldftn($Invoke1);
			*(data + 867100) = ldftn($Invoke2);
			*(data + 867128) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new EntityShaderSwapper((UIntPtr)0);
		}
	}
}
