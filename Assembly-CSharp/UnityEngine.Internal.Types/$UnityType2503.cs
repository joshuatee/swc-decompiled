using StaRTS.Main.Views.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2503 : $UnityType
	{
		public unsafe $UnityType2503()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 979408) = ldftn($Invoke0);
			*(data + 979436) = ldftn($Invoke1);
			*(data + 979464) = ldftn($Invoke2);
			*(data + 979492) = ldftn($Invoke3);
			*(data + 979520) = ldftn($Invoke4);
			*(data + 979548) = ldftn($Invoke5);
			*(data + 979576) = ldftn($Invoke6);
			*(data + 979604) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new FootprintMoveData((UIntPtr)0);
		}
	}
}
