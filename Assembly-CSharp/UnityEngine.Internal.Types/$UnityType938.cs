using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType938 : $UnityType
	{
		public unsafe $UnityType938()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 597516) = ldftn($Invoke0);
			*(data + 597544) = ldftn($Invoke1);
			*(data + 597572) = ldftn($Invoke2);
			*(data + 597600) = ldftn($Invoke3);
			*(data + 597628) = ldftn($Invoke4);
			*(data + 597656) = ldftn($Invoke5);
			*(data + 597684) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new ShowLoadingScreenPopupsStartupTask((UIntPtr)0);
		}
	}
}
