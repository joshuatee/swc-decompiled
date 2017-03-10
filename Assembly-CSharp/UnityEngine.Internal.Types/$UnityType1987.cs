using StaRTS.Main.RUF.RUFTasks;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1987 : $UnityType
	{
		public unsafe $UnityType1987()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 835908) = ldftn($Invoke0);
			*(data + 835936) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new CallsignRUFTask((UIntPtr)0);
		}
	}
}
