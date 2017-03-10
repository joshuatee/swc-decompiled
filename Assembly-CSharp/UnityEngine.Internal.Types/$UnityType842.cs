using StaRTS.Main.Controllers.Holonet;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType842 : $UnityType
	{
		public unsafe $UnityType842()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 575088) = ldftn($Invoke0);
			*(data + 575116) = ldftn($Invoke1);
			*(data + 575144) = ldftn($Invoke2);
			*(data + 575172) = ldftn($Invoke3);
			*(data + 575200) = ldftn($Invoke4);
			*(data + 575228) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new CommandCenterHolonetController((UIntPtr)0);
		}
	}
}
