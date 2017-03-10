using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1711 : $UnityType
	{
		public unsafe $UnityType1711()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 670092) = ldftn($Invoke0);
			*(data + 670120) = ldftn($Invoke1);
			*(data + 670148) = ldftn($Invoke2);
			*(data + 670176) = ldftn($Invoke3);
			*(data + 670204) = ldftn($Invoke4);
			*(data + 670232) = ldftn($Invoke5);
			*(data + 670260) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new HealthComponent((UIntPtr)0);
		}
	}
}
