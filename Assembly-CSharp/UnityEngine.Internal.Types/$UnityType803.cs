using StaRTS.Main.Controllers.Entities.StateMachines.Attack;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType803 : $UnityType
	{
		public unsafe $UnityType803()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 571336) = ldftn($Invoke0);
			*(data + 571364) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new CooldownState((UIntPtr)0);
		}
	}
}
