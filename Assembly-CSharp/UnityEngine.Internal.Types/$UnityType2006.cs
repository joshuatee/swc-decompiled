using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2006 : $UnityType
	{
		public unsafe $UnityType2006()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 837056) = ldftn($Invoke0);
			*(data + 837084) = ldftn($Invoke1);
			*(data + 837112) = ldftn($Invoke2);
			*(data + 837140) = ldftn($Invoke3);
			*(data + 837168) = ldftn($Invoke4);
			*(data + 837196) = ldftn($Invoke5);
			*(data + 837224) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new AbstractStoryAction((UIntPtr)0);
		}
	}
}
