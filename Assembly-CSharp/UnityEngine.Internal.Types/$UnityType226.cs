using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType226 : $UnityType
	{
		public unsafe $UnityType226()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 412576) = ldftn($Invoke0);
			*(data + 412604) = ldftn($Invoke1);
			*(data + 412632) = ldftn($Invoke2);
			*(data + 412660) = ldftn($Invoke3);
			*(data + 412688) = ldftn($Invoke4);
			*(data + 412716) = ldftn($Invoke5);
			*(data + 412744) = ldftn($Invoke6);
			*(data + 1529000) = ldftn($Get0);
			*(data + 1529004) = ldftn($Set0);
			*(data + 1529016) = ldftn($Get1);
			*(data + 1529020) = ldftn($Set1);
			*(data + 1529032) = ldftn($Get2);
			*(data + 1529036) = ldftn($Set2);
			*(data + 1529048) = ldftn($Get3);
			*(data + 1529052) = ldftn($Set3);
		}

		public override object CreateInstance()
		{
			return new UIViewport((UIntPtr)0);
		}
	}
}
