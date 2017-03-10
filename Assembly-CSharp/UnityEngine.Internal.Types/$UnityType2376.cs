using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2376 : $UnityType
	{
		public unsafe $UnityType2376()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 940264) = ldftn($Invoke0);
			*(data + 940292) = ldftn($Invoke1);
			*(data + 940320) = ldftn($Invoke2);
			*(data + 940348) = ldftn($Invoke3);
			*(data + 940376) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new TurretInfoScreen((UIntPtr)0);
		}
	}
}
