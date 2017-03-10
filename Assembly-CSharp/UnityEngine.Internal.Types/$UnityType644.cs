using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType644 : $UnityType
	{
		public unsafe $UnityType644()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 513068) = ldftn($Invoke0);
			*(data + 513096) = ldftn($Invoke1);
			*(data + 513124) = ldftn($Invoke2);
			*(data + 513152) = ldftn($Invoke3);
			*(data + 513180) = ldftn($Invoke4);
			*(data + 513208) = ldftn($Invoke5);
			*(data + 513236) = ldftn($Invoke6);
			*(data + 513264) = ldftn($Invoke7);
			*(data + 513292) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new SingleEmitterPool((UIntPtr)0);
		}
	}
}
