using StaRTS.Main.Controllers.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType974 : $UnityType
	{
		public unsafe $UnityType974()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 601772) = ldftn($Invoke0);
			*(data + 601800) = ldftn($Invoke1);
			*(data + 601828) = ldftn($Invoke2);
			*(data + 601856) = ldftn($Invoke3);
			*(data + 601884) = ldftn($Invoke4);
			*(data + 601912) = ldftn($Invoke5);
			*(data + 601940) = ldftn($Invoke6);
			*(data + 601968) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new HomeMapDataLoader((UIntPtr)0);
		}
	}
}
