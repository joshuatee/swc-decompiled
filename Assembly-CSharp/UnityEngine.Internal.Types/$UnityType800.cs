using StaRTS.Main.Controllers.Entities.StateMachines;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType800 : $UnityType
	{
		public unsafe $UnityType800()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 570132) = ldftn($Invoke0);
			*(data + 570160) = ldftn($Invoke1);
			*(data + 570188) = ldftn($Invoke2);
			*(data + 570216) = ldftn($Invoke3);
			*(data + 570244) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new TimeLockedStateMachine((UIntPtr)0);
		}
	}
}
