using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1049 : $UnityType
	{
		public unsafe $UnityType1049()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 626048) = ldftn($Invoke0);
			*(data + 626076) = ldftn($Invoke1);
			*(data + 626104) = ldftn($Invoke2);
			*(data + 626132) = ldftn($Invoke3);
			*(data + 626160) = ldftn($Invoke4);
			*(data + 626188) = ldftn($Invoke5);
			*(data + 626216) = ldftn($Invoke6);
			*(data + 626244) = ldftn($Invoke7);
			*(data + 626272) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new LevelMap((UIntPtr)0);
		}
	}
}
