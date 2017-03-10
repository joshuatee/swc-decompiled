using StaRTS.Utils.Scheduling;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2593 : $UnityType
	{
		public unsafe $UnityType2593()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 999204) = ldftn($Invoke0);
			*(data + 999232) = ldftn($Invoke1);
			*(data + 999260) = ldftn($Invoke2);
			*(data + 999288) = ldftn($Invoke3);
			*(data + 999316) = ldftn($Invoke4);
			*(data + 999344) = ldftn($Invoke5);
			*(data + 999372) = ldftn($Invoke6);
			*(data + 999400) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new Timer((UIntPtr)0);
		}
	}
}
