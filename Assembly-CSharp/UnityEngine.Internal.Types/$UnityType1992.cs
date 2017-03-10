using StaRTS.Main.RUF.RUFTasks;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1992 : $UnityType
	{
		public unsafe $UnityType1992()
		{
			*(UnityEngine.Internal.$Metadata.data + 836132) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new HolonetRUFTask((UIntPtr)0);
		}
	}
}
