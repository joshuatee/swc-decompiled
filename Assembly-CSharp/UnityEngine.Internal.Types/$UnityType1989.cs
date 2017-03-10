using StaRTS.Main.RUF.RUFTasks;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1989 : $UnityType
	{
		public unsafe $UnityType1989()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 835992) = ldftn($Invoke0);
			*(data + 836020) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new FueResumeRUFTask((UIntPtr)0);
		}
	}
}
