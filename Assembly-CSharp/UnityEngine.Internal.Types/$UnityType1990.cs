using StaRTS.Main.RUF.RUFTasks;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1990 : $UnityType
	{
		public unsafe $UnityType1990()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 836048) = ldftn($Invoke0);
			*(data + 836076) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new GoToHomeStateRUFTask((UIntPtr)0);
		}
	}
}
