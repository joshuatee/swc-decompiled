using StaRTS.Main.Views.Entities;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2186 : $UnityType
	{
		public unsafe $UnityType2186()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 869340) = ldftn($Invoke0);
			*(data + 869368) = ldftn($Invoke1);
			*(data + 869396) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new ShaderSwappedEntity((UIntPtr)0);
		}
	}
}
