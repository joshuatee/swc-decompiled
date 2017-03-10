using StaRTS.Main.RUF.RUFTasks;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1986 : $UnityType
	{
		public unsafe $UnityType1986()
		{
			*(UnityEngine.Internal.$Metadata.data + 835880) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new AutoTriggerRUFTask((UIntPtr)0);
		}
	}
}
