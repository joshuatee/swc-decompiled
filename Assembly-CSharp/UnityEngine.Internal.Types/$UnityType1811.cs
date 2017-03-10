using StaRTS.Main.Models.Planets;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1811 : $UnityType
	{
		public unsafe $UnityType1811()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 688292) = ldftn($Invoke0);
			*(data + 688320) = ldftn($Invoke1);
			*(data + 688348) = ldftn($Invoke2);
			*(data + 688376) = ldftn($Invoke3);
			*(data + 688404) = ldftn($Invoke4);
			*(data + 688432) = ldftn($Invoke5);
			*(data + 688460) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new PlanetRef((UIntPtr)0);
		}
	}
}
