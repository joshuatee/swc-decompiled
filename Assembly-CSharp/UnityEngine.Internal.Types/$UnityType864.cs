using StaRTS.Main.Controllers.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType864 : $UnityType
	{
		public unsafe $UnityType864()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 581332) = ldftn($Invoke0);
			*(data + 581360) = ldftn($Invoke1);
			*(data + 581388) = ldftn($Invoke2);
			*(data + 581416) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new AbstractObjectiveProcessor((UIntPtr)0);
		}
	}
}
