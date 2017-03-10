using StaRTS.Main.Utils.Events;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2134 : $UnityType
	{
		public unsafe $UnityType2134()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 859176) = ldftn($Invoke0);
			*(data + 859204) = ldftn($Invoke1);
			*(data + 859232) = ldftn($Invoke2);
			*(data + 859260) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new EventObservers((UIntPtr)0);
		}
	}
}
