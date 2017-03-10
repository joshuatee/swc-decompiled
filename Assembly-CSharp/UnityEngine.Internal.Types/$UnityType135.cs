using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType135 : $UnityType
	{
		public unsafe $UnityType135()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 381076) = ldftn($Invoke0);
			*(data + 381104) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new UICamera.MouseOrTouch((UIntPtr)0);
		}
	}
}
