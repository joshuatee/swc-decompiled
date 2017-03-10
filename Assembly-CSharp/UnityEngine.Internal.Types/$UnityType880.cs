using StaRTS.Main.Controllers.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType880 : $UnityType
	{
		public unsafe $UnityType880()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 582928) = ldftn($Invoke0);
			*(data + 582956) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new TrainSpecialAttackObjectiveProcessor((UIntPtr)0);
		}
	}
}
