using StaRTS.Utils.State;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2602 : $UnityType
	{
		public unsafe $UnityType2602()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 1000044) = ldftn($Invoke0);
			*(data + 1000072) = ldftn($Invoke1);
			*(data + 1000100) = ldftn($Invoke2);
			*(data + 1000128) = ldftn($Invoke3);
			*(data + 1000156) = ldftn($Invoke4);
			*(data + 1000184) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new StateMachine((UIntPtr)0);
		}
	}
}
