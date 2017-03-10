using StaRTS.Main.Views.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2517 : $UnityType
	{
		public unsafe $UnityType2517()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 985232) = ldftn($Invoke0);
			*(data + 985260) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new TrapRadiusView((UIntPtr)0);
		}
	}
}
