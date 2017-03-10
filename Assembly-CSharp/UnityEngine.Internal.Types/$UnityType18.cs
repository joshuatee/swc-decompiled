using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType18 : $UnityType
	{
		public unsafe $UnityType18()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 349660) = ldftn($Invoke0);
			*(data + 349688) = ldftn($Invoke1);
			*(data + 349716) = ldftn($Invoke2);
			*(data + 349744) = ldftn($Invoke3);
			*(data + 349772) = ldftn($Invoke4);
			*(data + 349800) = ldftn($Invoke5);
			*(data + 349828) = ldftn($Invoke6);
			*(data + 349856) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new BMGlyph((UIntPtr)0);
		}
	}
}
