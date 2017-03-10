using StaRTS.Main.Controllers.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType991 : $UnityType
	{
		public unsafe $UnityType991()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 604684) = ldftn($Invoke0);
			*(data + 604712) = ldftn($Invoke1);
			*(data + 604740) = ldftn($Invoke2);
			*(data + 604768) = ldftn($Invoke3);
			*(data + 604796) = ldftn($Invoke4);
			*(data + 604824) = ldftn($Invoke5);
			*(data + 604852) = ldftn($Invoke6);
			*(data + 604880) = ldftn($Invoke7);
			*(data + 604908) = ldftn($Invoke8);
			*(data + 604936) = ldftn($Invoke9);
			*(data + 604964) = ldftn($Invoke10);
			*(data + 604992) = ldftn($Invoke11);
			*(data + 605020) = ldftn($Invoke12);
			*(data + 605048) = ldftn($Invoke13);
			*(data + 605076) = ldftn($Invoke14);
			*(data + 605104) = ldftn($Invoke15);
			*(data + 605132) = ldftn($Invoke16);
			*(data + 605160) = ldftn($Invoke17);
		}

		public override object CreateInstance()
		{
			return new WorldTransitioner((UIntPtr)0);
		}
	}
}
