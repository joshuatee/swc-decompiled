using StaRTS.Main.Controllers.Holonet;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType848 : $UnityType
	{
		public unsafe $UnityType848()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 576880) = ldftn($Invoke0);
			*(data + 576908) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new VideosHolonetController((UIntPtr)0);
		}
	}
}
