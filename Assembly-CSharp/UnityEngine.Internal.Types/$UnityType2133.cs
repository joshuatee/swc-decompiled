using StaRTS.Main.Utils.Events;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2133 : $UnityType
	{
		public unsafe $UnityType2133()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 859036) = ldftn($Invoke0);
			*(data + 859064) = ldftn($Invoke1);
			*(data + 859092) = ldftn($Invoke2);
			*(data + 859120) = ldftn($Invoke3);
			*(data + 859148) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new EventManager((UIntPtr)0);
		}
	}
}
