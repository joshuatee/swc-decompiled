using StaRTS.Main.Controllers.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType988 : $UnityType
	{
		public unsafe $UnityType988()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 604152) = ldftn($Invoke0);
			*(data + 604180) = ldftn($Invoke1);
			*(data + 604208) = ldftn($Invoke2);
			*(data + 604236) = ldftn($Invoke3);
			*(data + 604264) = ldftn($Invoke4);
			*(data + 604292) = ldftn($Invoke5);
			*(data + 604320) = ldftn($Invoke6);
			*(data + 604348) = ldftn($Invoke7);
			*(data + 604376) = ldftn($Invoke8);
			*(data + 604404) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new WorldInitializer((UIntPtr)0);
		}
	}
}
