using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType122 : $UnityType
	{
		public unsafe $UnityType122()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 376820) = ldftn($Invoke0);
			*(data + 376848) = ldftn($Invoke1);
			*(data + 376876) = ldftn($Invoke2);
			*(data + 376904) = ldftn($Invoke3);
			*(data + 376932) = ldftn($Invoke4);
			*(data + 376960) = ldftn($Invoke5);
			*(data + 1525048) = ldftn($Get0);
			*(data + 1525052) = ldftn($Set0);
			*(data + 1525064) = ldftn($Get1);
			*(data + 1525068) = ldftn($Set1);
		}

		public override object CreateInstance()
		{
			return new UIButtonActivate((UIntPtr)0);
		}
	}
}
