using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType685 : $UnityType
	{
		public unsafe $UnityType685()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 522980) = ldftn($Invoke0);
			*(data + 523008) = ldftn($Invoke1);
			*(data + 523036) = ldftn($Invoke2);
			*(data + 523064) = ldftn($Invoke3);
			*(data + 523092) = ldftn($Invoke4);
			*(data + 523120) = ldftn($Invoke5);
			*(data + 523148) = ldftn($Invoke6);
			*(data + 523176) = ldftn($Invoke7);
			*(data + 523204) = ldftn($Invoke8);
			*(data + 523232) = ldftn($Invoke9);
			*(data + 523260) = ldftn($Invoke10);
			*(data + 523288) = ldftn($Invoke11);
			*(data + 523316) = ldftn($Invoke12);
			*(data + 523344) = ldftn($Invoke13);
			*(data + 523372) = ldftn($Invoke14);
			*(data + 523400) = ldftn($Invoke15);
		}

		public override object CreateInstance()
		{
			return new BattleRecordController((UIntPtr)0);
		}
	}
}
