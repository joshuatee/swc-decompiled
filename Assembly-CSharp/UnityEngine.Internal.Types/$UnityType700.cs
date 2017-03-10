using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType700 : $UnityType
	{
		public unsafe $UnityType700()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 531660) = ldftn($Invoke0);
			*(data + 531688) = ldftn($Invoke1);
			*(data + 531716) = ldftn($Invoke2);
			*(data + 531744) = ldftn($Invoke3);
			*(data + 531772) = ldftn($Invoke4);
			*(data + 531800) = ldftn($Invoke5);
			*(data + 531828) = ldftn($Invoke6);
			*(data + 531856) = ldftn($Invoke7);
			*(data + 531884) = ldftn($Invoke8);
			*(data + 531912) = ldftn($Invoke9);
			*(data + 531940) = ldftn($Invoke10);
			*(data + 531968) = ldftn($Invoke11);
			*(data + 531996) = ldftn($Invoke12);
			*(data + 532024) = ldftn($Invoke13);
			*(data + 532052) = ldftn($Invoke14);
			*(data + 532080) = ldftn($Invoke15);
			*(data + 532108) = ldftn($Invoke16);
			*(data + 532136) = ldftn($Invoke17);
		}

		public override object CreateInstance()
		{
			return new DefensiveBattleController((UIntPtr)0);
		}
	}
}
