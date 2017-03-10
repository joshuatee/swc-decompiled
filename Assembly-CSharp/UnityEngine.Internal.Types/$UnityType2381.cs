using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2381 : $UnityType
	{
		public unsafe $UnityType2381()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 941216) = ldftn($Invoke0);
			*(data + 941244) = ldftn($Invoke1);
			*(data + 941272) = ldftn($Invoke2);
			*(data + 941300) = ldftn($Invoke3);
			*(data + 941328) = ldftn($Invoke4);
			*(data + 941356) = ldftn($Invoke5);
			*(data + 941384) = ldftn($Invoke6);
			*(data + 941412) = ldftn($Invoke7);
			*(data + 941440) = ldftn($Invoke8);
			*(data + 941468) = ldftn($Invoke9);
			*(data + 941496) = ldftn($Invoke10);
			*(data + 941524) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new VideosShareSquadScreen((UIntPtr)0);
		}
	}
}
