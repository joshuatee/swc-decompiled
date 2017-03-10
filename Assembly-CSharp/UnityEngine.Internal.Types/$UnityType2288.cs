using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2288 : $UnityType
	{
		public unsafe $UnityType2288()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 911424) = ldftn($Invoke0);
			*(data + 911452) = ldftn($Invoke1);
			*(data + 911480) = ldftn($Invoke2);
			*(data + 911508) = ldftn($Invoke3);
			*(data + 911536) = ldftn($Invoke4);
			*(data + 911564) = ldftn($Invoke5);
			*(data + 911592) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new ChampionInfoScreen((UIntPtr)0);
		}
	}
}
