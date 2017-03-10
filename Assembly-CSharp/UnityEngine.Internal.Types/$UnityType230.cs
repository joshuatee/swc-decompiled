using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType230 : $UnityType
	{
		public unsafe $UnityType230()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 414956) = ldftn($Invoke0);
			*(data + 414984) = ldftn($Invoke1);
			*(data + 415012) = ldftn($Invoke2);
			*(data + 415040) = ldftn($Invoke3);
			*(data + 415068) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new UIWidgetContainer((UIntPtr)0);
		}
	}
}
