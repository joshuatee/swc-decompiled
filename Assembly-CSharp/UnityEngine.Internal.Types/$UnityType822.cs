using StaRTS.Main.Controllers.Entities.Systems;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType822 : $UnityType
	{
		public unsafe $UnityType822()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 572680) = ldftn($Invoke0);
			*(data + 572708) = ldftn($Invoke1);
			*(data + 572736) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new TransportSystem((UIntPtr)0);
		}
	}
}
