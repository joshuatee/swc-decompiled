using StaRTS.Main.Controllers.Entities.StateMachines.Attack;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType806 : $UnityType
	{
		public unsafe $UnityType806()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 571448) = ldftn($Invoke0);
			*(data + 571476) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new PreFireState((UIntPtr)0);
		}
	}
}
