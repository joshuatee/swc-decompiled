using StaRTS.Main.Controllers.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType985 : $UnityType
	{
		public unsafe $UnityType985()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 603368) = ldftn($Invoke0);
			*(data + 603396) = ldftn($Invoke1);
			*(data + 603424) = ldftn($Invoke2);
			*(data + 603452) = ldftn($Invoke3);
			*(data + 603480) = ldftn($Invoke4);
			*(data + 603508) = ldftn($Invoke5);
			*(data + 603536) = ldftn($Invoke6);
			*(data + 603564) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new UserWarBaseMapDataLoader((UIntPtr)0);
		}
	}
}
