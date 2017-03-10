using StaRTS.Main.Controllers.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType879 : $UnityType
	{
		public unsafe $UnityType879()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 582872) = ldftn($Invoke0);
			*(data + 582900) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new TrainSpecialAttackIdObjectiveProcessor((UIntPtr)0);
		}
	}
}
