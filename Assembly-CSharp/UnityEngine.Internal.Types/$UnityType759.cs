using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType759 : $UnityType
	{
		public unsafe $UnityType759()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 551120) = ldftn($Invoke0);
			*(data + 551148) = ldftn($Invoke1);
			*(data + 551176) = ldftn($Invoke2);
			*(data + 551204) = ldftn($Invoke3);
			*(data + 551232) = ldftn($Invoke4);
			*(data + 551260) = ldftn($Invoke5);
			*(data + 551288) = ldftn($Invoke6);
			*(data + 551316) = ldftn($Invoke7);
			*(data + 551344) = ldftn($Invoke8);
			*(data + 551372) = ldftn($Invoke9);
			*(data + 551400) = ldftn($Invoke10);
			*(data + 551428) = ldftn($Invoke11);
			*(data + 551456) = ldftn($Invoke12);
			*(data + 551484) = ldftn($Invoke13);
			*(data + 551512) = ldftn($Invoke14);
			*(data + 551540) = ldftn($Invoke15);
			*(data + 551568) = ldftn($Invoke16);
			*(data + 551596) = ldftn($Invoke17);
		}

		public override object CreateInstance()
		{
			return new ShieldController((UIntPtr)0);
		}
	}
}
