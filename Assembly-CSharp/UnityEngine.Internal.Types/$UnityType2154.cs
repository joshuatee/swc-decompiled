using StaRTS.Main.Views.Animations;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2154 : $UnityType
	{
		public unsafe $UnityType2154()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 862340) = ldftn($Invoke0);
			*(data + 862368) = ldftn($Invoke1);
			*(data + 862396) = ldftn($Invoke2);
			*(data + 862424) = ldftn($Invoke3);
			*(data + 862452) = ldftn($Invoke4);
			*(data + 862480) = ldftn($Invoke5);
			*(data + 862508) = ldftn($Invoke6);
			*(data + 862536) = ldftn($Invoke7);
			*(data + 862564) = ldftn($Invoke8);
			*(data + 862592) = ldftn($Invoke9);
			*(data + 862620) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new TextCrawlAnimation((UIntPtr)0);
		}
	}
}
