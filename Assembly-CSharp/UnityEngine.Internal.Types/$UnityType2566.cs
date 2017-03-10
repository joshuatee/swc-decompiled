using StaRTS.Utils.Json;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2566 : $UnityType
	{
		public unsafe $UnityType2566()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 996208) = ldftn($Invoke0);
			*(data + 996236) = ldftn($Invoke1);
			*(data + 996264) = ldftn($Invoke2);
			*(data + 996292) = ldftn($Invoke3);
			*(data + 996320) = ldftn($Invoke4);
			*(data + 996348) = ldftn($Invoke5);
			*(data + 996376) = ldftn($Invoke6);
			*(data + 996404) = ldftn($Invoke7);
			*(data + 996432) = ldftn($Invoke8);
			*(data + 996460) = ldftn($Invoke9);
			*(data + 996488) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new JsonParser((UIntPtr)0);
		}
	}
}
