using StaRTS.Main.Views.UX.Tags;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2477 : $UnityType
	{
		public unsafe $UnityType2477()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 971568) = ldftn($Invoke0);
			*(data + 971596) = ldftn($Invoke1);
			*(data + 971624) = ldftn($Invoke2);
			*(data + 971652) = ldftn($Invoke3);
			*(data + 971680) = ldftn($Invoke4);
			*(data + 971708) = ldftn($Invoke5);
			*(data + 971736) = ldftn($Invoke6);
			*(data + 971764) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new QueuedUnitTrainingTag((UIntPtr)0);
		}
	}
}
