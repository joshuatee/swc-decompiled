using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType221 : $UnityType
	{
		public unsafe $UnityType221()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 411120) = ldftn($Invoke0);
			*(data + 411148) = ldftn($Invoke1);
			*(data + 411176) = ldftn($Invoke2);
			*(data + 411204) = ldftn($Invoke3);
			*(data + 411232) = ldftn($Invoke4);
			*(data + 411260) = ldftn($Invoke5);
			*(data + 411288) = ldftn($Invoke6);
			*(data + 411316) = ldftn($Invoke7);
			*(data + 1528792) = ldftn($Get0);
			*(data + 1528796) = ldftn($Set0);
			*(data + 1528808) = ldftn($Get1);
			*(data + 1528812) = ldftn($Set1);
		}

		public override object CreateInstance()
		{
			return new UIToggledObjects((UIntPtr)0);
		}
	}
}
