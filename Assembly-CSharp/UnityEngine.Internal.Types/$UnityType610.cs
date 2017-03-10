using StaRTS.Externals.Manimal.TransferObjects.Request;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType610 : $UnityType
	{
		public unsafe $UnityType610()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 505368) = ldftn($Invoke0);
			*(data + 505396) = ldftn($Invoke1);
			*(data + 505424) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new PlanetIdRequest((UIntPtr)0);
		}
	}
}
