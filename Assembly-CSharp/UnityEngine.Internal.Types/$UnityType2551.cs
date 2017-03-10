using StaRTS.Utils.Animation.Anims;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2551 : $UnityType
	{
		public unsafe $UnityType2551()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 994920) = ldftn($Invoke0);
			*(data + 994948) = ldftn($Invoke1);
			*(data + 994976) = ldftn($Invoke2);
			*(data + 995004) = ldftn($Invoke3);
			*(data + 995032) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new AnimScale((UIntPtr)0);
		}
	}
}
