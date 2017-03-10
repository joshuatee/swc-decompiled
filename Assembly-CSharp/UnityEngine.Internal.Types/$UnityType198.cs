using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType198 : $UnityType
	{
		public unsafe $UnityType198()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 404512) = ldftn($Invoke0);
			*(data + 404540) = ldftn($Invoke1);
			*(data + 404568) = ldftn($Invoke2);
			*(data + 404596) = ldftn($Invoke3);
			*(data + 404624) = ldftn($Invoke4);
			*(data + 404652) = ldftn($Invoke5);
			*(data + 404680) = ldftn($Invoke6);
			*(data + 404708) = ldftn($Invoke7);
			*(data + 404736) = ldftn($Invoke8);
			*(data + 404764) = ldftn($Invoke9);
			*(data + 404792) = ldftn($Invoke10);
			*(data + 404820) = ldftn($Invoke11);
			*(data + 404848) = ldftn($Invoke12);
			*(data + 1527816) = ldftn($Get0);
			*(data + 1527820) = ldftn($Set0);
			*(data + 1527832) = ldftn($Get1);
			*(data + 1527836) = ldftn($Set1);
		}

		public override object CreateInstance()
		{
			return new UIScrollBar((UIntPtr)0);
		}
	}
}
