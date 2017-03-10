using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2286 : $UnityType
	{
		public unsafe $UnityType2286()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 911060) = ldftn($Invoke0);
			*(data + 911088) = ldftn($Invoke1);
			*(data + 911116) = ldftn($Invoke2);
			*(data + 911144) = ldftn($Invoke3);
			*(data + 911172) = ldftn($Invoke4);
			*(data + 911200) = ldftn($Invoke5);
			*(data + 911228) = ldftn($Invoke6);
			*(data + 911256) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new CampaignCompleteScreen((UIntPtr)0);
		}
	}
}
