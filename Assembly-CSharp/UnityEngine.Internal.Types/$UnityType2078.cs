using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2078 : $UnityType
	{
		public unsafe $UnityType2078()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 842544) = ldftn($Invoke0);
			*(data + 842572) = ldftn($Invoke1);
			*(data + 842600) = ldftn($Invoke2);
			*(data + 842628) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new TextCrawlStoryAction((UIntPtr)0);
		}
	}
}
