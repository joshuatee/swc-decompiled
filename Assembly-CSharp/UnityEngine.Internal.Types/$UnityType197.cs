using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType197 : $UnityType
	{
		public unsafe $UnityType197()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 404176) = ldftn($Invoke0);
			*(data + 404204) = ldftn($Invoke1);
			*(data + 404232) = ldftn($Invoke2);
			*(data + 404260) = ldftn($Invoke3);
			*(data + 404288) = ldftn($Invoke4);
			*(data + 404316) = ldftn($Invoke5);
			*(data + 404344) = ldftn($Invoke6);
			*(data + 404372) = ldftn($Invoke7);
			*(data + 404400) = ldftn($Invoke8);
			*(data + 404428) = ldftn($Invoke9);
			*(data + 404456) = ldftn($Invoke10);
			*(data + 404484) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new UISavedOption((UIntPtr)0);
		}
	}
}
