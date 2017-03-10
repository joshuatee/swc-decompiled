using StaRTS.Main.Views.Projectors;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2199 : $UnityType
	{
		public unsafe $UnityType2199()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 870852) = ldftn($Invoke0);
			*(data + 870880) = ldftn($Invoke1);
			*(data + 870908) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new ProjectorConfig((UIntPtr)0);
		}
	}
}
