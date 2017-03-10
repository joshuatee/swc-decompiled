using StaRTS.Main.RUF.RUFTasks;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1985 : $UnityType
	{
		public unsafe $UnityType1985()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 835572) = ldftn($Invoke0);
			*(data + 835600) = ldftn($Invoke1);
			*(data + 835628) = ldftn($Invoke2);
			*(data + 835656) = ldftn($Invoke3);
			*(data + 835684) = ldftn($Invoke4);
			*(data + 835712) = ldftn($Invoke5);
			*(data + 835740) = ldftn($Invoke6);
			*(data + 835768) = ldftn($Invoke7);
			*(data + 835796) = ldftn($Invoke8);
			*(data + 835824) = ldftn($Invoke9);
			*(data + 835852) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new AbstractRUFTask((UIntPtr)0);
		}
	}
}
